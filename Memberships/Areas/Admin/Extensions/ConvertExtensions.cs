using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace Memberships.Areas.Admin.Extensions
{
    public static class ConvertExtensions
    {
        public static async Task<IEnumerable<ProductModel>>Convert(this IEnumerable<Product> Products, ApplicationDbContext db)
        {
            if (Products.Count().Equals(0))
                return new List<ProductModel>();

            var texts = await db.ProductLinkTexts.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return from p in Products
                   select new ProductModel
                   {
                       Id = p.Id,
                       Title = p.Title,
                       Description = p.Description,
                       ImageUrl = p.ImageUrl,
                       ProductLinkTextId = p.ProductLinkTextId,
                       ProductTypeId = p.ProductTypeId,
                       ProductTypes = types,
                       ProductLinkTexts = texts
                   };
        }

        public static async Task<ProductModel> Convert(this Product Product, ApplicationDbContext db)
        {

            var text = await db.ProductLinkTexts.FirstOrDefaultAsync(
                p => p.Id.Equals(Product.ProductLinkTextId));
            var type = await db.ProductTypes.FirstOrDefaultAsync(
                p=> p.Id.Equals(Product.ProductTypeId));

           
                   var model = new ProductModel
                   {
                       Id = Product.Id,
                       Title = Product.Title,
                       Description = Product.Description,
                       ImageUrl = Product.ImageUrl,
                       ProductLinkTextId = Product.ProductLinkTextId,
                       ProductTypeId = Product.ProductTypeId,
                       ProductTypes = new List<ProductType>(),
                       ProductLinkTexts = new List<ProductLinkText>()
                   };
            model.ProductTypes.Add(type);
            model.ProductLinkTexts.Add(text);

            return model;
        }

        public static async Task<IEnumerable<ProductItemModel>> Convert(this IQueryable<ProductItem> ProductItems, ApplicationDbContext db)
        {
            if (ProductItems.Count().Equals(0))
                return new List<ProductItemModel>();

            var texts =  db.ProductItems.ToListAsync();
            var types =  db.ProductTypes.ToListAsync();

            return await (from pi in ProductItems
                          select new ProductItemModel
                          {
                              ItemId = pi.ItemId,
                              ProductId = pi.ProductId,

                              ItemTitle = db.Items.FirstOrDefault(i => i.Id.Equals(pi.ItemId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(i => i.Id.Equals(pi.ProductId)).Title
                          }).ToListAsync();
        }

        public static async Task<ProductItemModel> Convert(this ProductItem ProductItem, ApplicationDbContext db)
        {
            var model = new ProductItemModel
            {
              ItemId = ProductItem.ItemId,
              ProductId = ProductItem.ProductId,
              Items = await db.Items.ToListAsync(),
                Products = await db.Products.ToListAsync()
            };
           return model;
        }

    }
}