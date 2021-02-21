using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using BookStore_DataAccess;
using BookStore_Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore_Models.ViewModels;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using BookStore_Utility;
using BookStore_DataAccess.Repository.IRepository;

namespace BookStore.Controllers
{
    [Authorize(Roles = WC.AdminRole)]
    public class ProductController : Controller
    {
        private readonly IProductRepository _productRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IProductRepository productRepository, IWebHostEnvironment webHostEnvironment)
        {
            _productRepository = productRepository;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            IEnumerable<Product> objList = _productRepository.GetAll(includeProperties:"Category,ApplicationType");

            //foreach (var item in objList)
            //{
            //    item.Category = _db.Categories.FirstOrDefault(l => l.Id == item.CategoryId);
            //    item.ApplicationType = _db.ApplicationTypes.FirstOrDefault(l => l.Id == item.ApplicationTypeId);
            //}


            return View(objList);
        }


        //GET - UPSERT
        public IActionResult Upsert(int? id)
        {
            #region Refact
            /*
            IEnumerable<SelectListItem> CategoryDropDown =
                _db.Categories.Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });

            //ViewBag.CategoryDropDown = CategoryDropDown;
            ViewData["CategoryDropDown"] = CategoryDropDown;

            Product product = new Product();
            */
            #endregion
            //use stronglytype
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategorySelectList = _productRepository.GetAllDropdownList(WC.CategoryName),
                ApplicationTypeSelectList = _productRepository.GetAllDropdownList(WC.ApplicationTypeName)

            };


            if (id == null) 
            {
                //this is for create
                return View(productVM);
            }
            else
            {
                productVM.Product = _productRepository.Find(id.GetValueOrDefault());
                if(productVM.Product == null)
                {
                    return NotFound();
                }
                return View(productVM);
            }
        }

        //POST - UPSERT
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                //_db.Categories.Add(obj);
                //_db.SaveChanges();

                var files = HttpContext.Request.Form.Files;
                string webRootPath = _webHostEnvironment.WebRootPath;

                if(productVM.Product.Id == 0) 
                {
                    //Creating
                    string upload = webRootPath + WC.ImagePath;
                    string fileName = Guid.NewGuid().ToString();
                    string extension = Path.GetExtension(files[0].FileName);

                    using (var fileStrem = new FileStream(Path.Combine(upload,fileName+extension),FileMode.Create))
                    {
                        files[0].CopyTo(fileStrem);
                    }

                    productVM.Product.Image = fileName + extension;

                    _productRepository.Add(productVM.Product);
                }
                else 
                {
                    //Updating
                    var objFromDb = _productRepository.FirstOrDefault(l => l.Id == productVM.Product.Id, isTracking: false);

                    if (files.Count > 0)
                    {
                        string upload = webRootPath + WC.ImagePath;
                        string fileName = Guid.NewGuid().ToString();
                        string extension = Path.GetExtension(files[0].FileName);

                        var oldFile = Path.Combine(upload, objFromDb.Image);
                        if (System.IO.File.Exists(oldFile)) { System.IO.File.Delete(oldFile); }

                        using (var fileStrem = new FileStream(Path.Combine(upload, fileName + extension), FileMode.Create))
                        {
                            files[0].CopyTo(fileStrem);
                        }

                        productVM.Product.Image = fileName + extension;
                    }
                    else
                    {
                        productVM.Product.Image = objFromDb.Image;
                    }
                    _productRepository.Update(productVM.Product);
                }

                _productRepository.Save();
                return RedirectToAction("Index");
            }

            productVM.CategorySelectList = _productRepository.GetAllDropdownList(WC.CategoryName);
            productVM.ApplicationTypeSelectList = _productRepository.GetAllDropdownList(WC.ApplicationTypeName);

            return View(productVM); 
            
        }


        //GET - DELETE
        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product product = _productRepository.FirstOrDefault(l=>l.Id==id, includeProperties:"Category,ApplicationType");
            //product.Category = _db.Categories.Find(product.CategoryId);
            
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        //POST - DELETE
        [HttpPost,ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeletePost(int? id)
        {
            var obj = _productRepository.Find(id.GetValueOrDefault());
            if (obj == null) { return NotFound(); }

            string upload =_webHostEnvironment.WebRootPath + WC.ImagePath;
            var oldFile = Path.Combine(upload, obj.Image);
            if (System.IO.File.Exists(oldFile)) { System.IO.File.Delete(oldFile); }

            _productRepository.Remove(obj);
            _productRepository.Save();
            return RedirectToAction("Index");

        }
    }
}
