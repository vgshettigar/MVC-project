﻿using System.Threading.Tasks;
using System.Net;
using System.Web.Mvc;
using Memberships.Entities;
using Memberships.Models;
using Memberships.Areas.Admin.Extensions;
using Memberships.Areas.Admin.Models;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;

namespace Memberships.Areas.Admin.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductController : Controller
    {
        
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: Admin/Product
        public async Task<ActionResult> Index()
        {
            var products = await db.Products.ToListAsync();
            var model = await products.Convert(db);
            return View(model);
        }

        // GET: Admin/Product/Details/5
        public async Task<ActionResult> Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }           
            var ProductModel = await product.Convert(db);
            return View(ProductModel);
        }

        // GET: Admin/Product/Create
        public async Task<ActionResult> Create()
        {
            var model = new ProductModel
            {
                ProductLinkTexts = await db.ProductLinkTexts.ToListAsync(),
                ProductTypes = await db.ProductTypes.ToListAsync()
            };
        return View(model);
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind(Include = "Id,Title,Description,ImageUrl,ProductLinkTextId,ProductTypeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            return View(product);
        }

        // GET: Admin/Product/Edit/5
        public async Task<ActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var prod = new List<Product>();
            prod.Add(product);
            var prodModel = await prod.Convert(db);
            return View(prodModel.First());
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit([Bind(Include = "Id,Title,Description,ImageUrl,ProductLinkTextId,ProductTypeId")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                await db.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        // GET: Admin/Product/Delete/5
        public async Task<ActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = await db.Products.FindAsync(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            var ProductModel = await product.Convert(db);
            return View(ProductModel);
        }

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> DeleteConfirmed(int id)
        {
            Product product = await db.Products.FindAsync(id);
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
