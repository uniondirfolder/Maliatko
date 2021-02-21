using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookStore_DataAccess;
using BookStore_Models;
using Microsoft.AspNetCore.Authorization;
using BookStore_Utility;
using BookStore_DataAccess.Repository.IRepository;

namespace BookStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ApplicationTypeController : Controller
    {
        private readonly IApplicationTypeRepository _applicationTypeRepository;

        public ApplicationTypeController(IApplicationTypeRepository applicationTypeRepository)
        {
            _applicationTypeRepository = applicationTypeRepository;
        }

        public IActionResult Index()
        {
            IEnumerable<ApplicationType> objList = _applicationTypeRepository.GetAll();
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
        public IActionResult Create(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _applicationTypeRepository.Add(obj);
                _applicationTypeRepository.Save();
                return RedirectToAction("Index");
            }
            return View(obj);

        }

        //GET - EDIT
        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            var obj = _applicationTypeRepository.Find(id.GetValueOrDefault());
            if (obj == null)
            {
                return NotFound();
            }
            return View(obj);
        }

        //POST - EDIT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(ApplicationType obj)
        {
            if (ModelState.IsValid)
            {
                _applicationTypeRepository.Update(obj);
                _applicationTypeRepository.Save();
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
            var obj = _applicationTypeRepository.Find(id.GetValueOrDefault());
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
            var obj = _applicationTypeRepository.Find(id.GetValueOrDefault());
            if (obj == null) { return NotFound(); }

            _applicationTypeRepository.Remove(obj);
            _applicationTypeRepository.Save();
            return RedirectToAction("Index");

        }
    }
}
