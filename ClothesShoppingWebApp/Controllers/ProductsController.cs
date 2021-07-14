﻿using DAOLibrary.Repository.Interface;
using DAOLibrary.Repository.Object;
using DTOLibrary;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClothesShoppingWebApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController : Controller
    {
        IProductRepository productRepository = null;
        ICategoryRepository categoryRepository = null;
        public ProductsController()
        {
            productRepository = new ProductRepository();
            categoryRepository = new CategoryRepository();
        }
        [Authorize(Roles = "Admin")]
        // GET: ProductsController
        public ActionResult Index(string status, string categoryId)
        {
            var productList = productRepository.GetProductList();
            
            if(!string.IsNullOrEmpty(categoryId))
            {
                productList = productList.Where(p => p.CategoryId.ToString() == categoryId);
            }
            if(status == "true")
            {
                productList = productList.Where(p => p.Status == true);
            } else if(status == "false")
            {
                productList = productList.Where(p => p.Status == false);
            }

            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
            return View(productList);
        }
        [Authorize(Roles = "Admin")]
        // GET: ProductsController/Details/5
        public ActionResult Details(int id)
        {       
            var product = productRepository.GetProductById(id);
            if (product == null)
            {
                return NotFound();
            }
            return View(product);
        }

        // GET: ProductsController/Create
        [Authorize(Roles = "Admin")]
        public ActionResult Create()
        {
            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName");
            return View();
        }

        // POST: ProductsController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Product product)
        {
            try
            {
                if(ModelState.IsValid)
                {
                    productRepository.CreatePostProduct(product);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                return View(product);
            }
        }

        // GET: ProductsController/Edit/5
        [Authorize(Roles = "Admin")]
        public ActionResult Edit(int id)
        {
            Product product = productRepository.GetProductById(id);
            ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName", product.CategoryId);
            return View(product);
        }

        [Authorize(Roles = "Admin")]
        // POST: ProductsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Product product)
        {
            try
            {
                if (id != product.ProductId)
                {
                    return NotFound();
                }

                if(ModelState.IsValid)
                {
                    productRepository.UpdateProduct(product);
                }

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.Message;
                ViewData["Category"] = new SelectList(categoryRepository.GetCategoryList(), "CategoryId", "CategoryName", product.CategoryId);
                return View();
            }
        }

    }
}
