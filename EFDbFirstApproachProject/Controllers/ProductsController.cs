using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EFDbFirstApproachProject.Models;
namespace EFDbFirstApproachProject.Controllers
{
    public class ProductsController : Controller
    {
        // GET: Products
        public ActionResult Index(string search="")
        {
            EFDBFirstDatabaseEntities db=new EFDBFirstDatabaseEntities();
            ViewBag.Search = search;
            List<Product> products= db.Products.Where(temp=>temp.ProductName.Contains(search)).ToList();
            return View(products);
        }

        public ActionResult Details(long id)
        {
            EFDBFirstDatabaseEntities db= new EFDBFirstDatabaseEntities();  
            Product prdt=db.Products.Where(temp=>temp.ProductID==id).FirstOrDefault();

            return View(prdt);
        }

        public ActionResult Create()
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            ViewBag.Category = db.Categories.ToList();
            ViewBag.Brand = db.Brands.ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            EFDBFirstDatabaseEntities db=new EFDBFirstDatabaseEntities();
            if (Request.Files.Count >= 1)
            {
                var file = Request.Files[0];
                var imageBytes = new Byte[file.ContentLength - 1];
                file.InputStream.Read(imageBytes, 0, imageBytes.Length);
                var base64String=Convert.ToBase64String(imageBytes,0,imageBytes.Length);
                product.Photo = base64String;
            }
      
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("index");
        }

        public ActionResult Edit(long id)
        {
            EFDBFirstDatabaseEntities db= new EFDBFirstDatabaseEntities();
            Product existingProduct=db.Products.Where(item=>item.ProductID==id).FirstOrDefault();
            ViewBag.Category = db.Categories.ToList();
            ViewBag.Brand = db.Brands.ToList();
            return View(existingProduct);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            EFDBFirstDatabaseEntities db= new EFDBFirstDatabaseEntities();
            Product existingProduct=db.Products.Where(item=>item.ProductID==product.ProductID).FirstOrDefault();
            existingProduct.ProductName= product.ProductName;  
            existingProduct.DateOfPurchase= product.DateOfPurchase;
            existingProduct.Price=product.Price;
            existingProduct.Active=product.Active;
            existingProduct.AvailabilityStatus=product.AvailabilityStatus;
            existingProduct.BrandID= product.BrandID;
            existingProduct.CategoryID=product.CategoryID;
            db.SaveChanges();
            return RedirectToAction("index","products");
        }

        public ActionResult Delete(long id)
        {
            EFDBFirstDatabaseEntities db=new EFDBFirstDatabaseEntities();
            Product machingData = db.Products.Where(item => item.ProductID == id).FirstOrDefault();
            return View(machingData);
        }
        [HttpPost]
        public ActionResult Delete(long id, Product product)
        {
            EFDBFirstDatabaseEntities db = new EFDBFirstDatabaseEntities();
            Product machingData=db.Products.Where(item=>item.ProductID==id).FirstOrDefault();
            db.Products.Remove(machingData);
            db.SaveChanges();
            return RedirectToAction("index","products");
        }
    }
}