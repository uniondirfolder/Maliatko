using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using BookStore_DataAccess;
using BookStore_Models;
using Microsoft.AspNetCore.Authorization;
using BookStore_Utility;
using BookStore_DataAccess.Repository.IRepository;

namespace BookStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<Category> objList = _categoryRepository.GetAll();
            return View(objList);
        }


        //GET - CREATE
        public IActionResult Create()
        {
            return View();
        }

        //POST - CREATE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Add(obj);
                _categoryRepository.Save();
                return RedirectToAction("Index");
            }
            return View(obj); 
            
        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if(id==null || id == 0) 
            {
                return NotFound();
            }
            var obj = _categoryRepository.Find(id.GetValueOrDefault());
            if (obj == null) 
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(obj);
                _categoryRepository.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _categoryRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - DELETE
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _categoryRepository.Find(id.GetValueOrDefault());
            if (obj == null) { return NotFound(); }

            _categoryRepository.Remove(obj);
            _categoryRepository.Save();
            return RedirectToAction("Index");

        }
    }
}
