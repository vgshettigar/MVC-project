using Memberships.Areas.Admin.Models;
using Memberships.Entities;
using Memberships.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web;

namespace Memberships.Areas.Admin.Extensions
{
    public static class ConvertExtensions
    {
        #region Product
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

        #endregion

        #region ProductItem
        public static async Task<IEnumerable<ProductItemModel>> Convert(this IQueryable<ProductItem> ProductItems, ApplicationDbContext db)
        {
            if (ProductItems.Count().Equals(0))
                return new List<ProductItemModel>();

            var texts = await db.ProductItems.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return await (from pi in ProductItems
                          select new ProductItemModel
                          {
                              ItemId = pi.ItemId,
                              ProductId = pi.ProductId,

                              ItemTitle = db.Items.FirstOrDefault(i => i.Id.Equals(pi.ItemId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(i => i.Id.Equals(pi.ProductId)).Title
                          }).ToListAsync();
        }

        public static async Task<ProductItemModel> Convert(this ProductItem ProductItem, ApplicationDbContext db,
            bool addListData=true)
        {
            var model = new ProductItemModel
            {
                ItemId = ProductItem.ItemId,
                ProductId = ProductItem.ProductId,
                Items = addListData ? await db.Items.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                ItemTitle = (await db.Items.FirstOrDefaultAsync(i => i.Id.Equals(ProductItem.ItemId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(i => i.Id.Equals(ProductItem.ProductId))).Title
            };
           return model;
        }

        public static async Task<bool>CanChange(this ProductItem ProductItem, ApplicationDbContext db)
        {
            var OldPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(ProductItem.OldProductId) &&
            pi.ItemId.Equals(ProductItem.OldItemId));

            var NewPI = await db.ProductItems.CountAsync(pi => pi.ProductId.Equals(ProductItem.ProductId) &&
            pi.ItemId.Equals(ProductItem.ItemId));

            return OldPI.Equals(1) && NewPI.Equals(0);
        }

        public static async Task Change(this ProductItem ProductItem, ApplicationDbContext db)
        {
            var OldProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(ProductItem.OldProductId) &&
            pi.ItemId.Equals(ProductItem.OldItemId));

            var NewProductItem = await db.ProductItems.FirstOrDefaultAsync(pi => pi.ProductId.Equals(ProductItem.ProductId) &&
            pi.ItemId.Equals(ProductItem.ItemId));

            if(OldProductItem != null && NewProductItem == null)
            {
                NewProductItem = new ProductItem
                {
                    ItemId = ProductItem.ItemId,
                    ProductId = ProductItem.ProductId
                };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    db.ProductItems.Remove(OldProductItem);
                    db.ProductItems.Add(NewProductItem);

                    await db.SaveChangesAsync();
                    transaction.Complete();
                }
            }
        }

        #endregion

        #region SubscritptionProduct
        public static async Task<IEnumerable<SubscriptionProductModel>> Convert(this IQueryable<SubscriptionProduct> subscriptionProducts, ApplicationDbContext db)
        {
            if (subscriptionProducts.Count().Equals(0))
                return new List<SubscriptionProductModel>();

            var texts = await db.ProductItems.ToListAsync();
            var types = await db.ProductTypes.ToListAsync();

            return await (from pi in subscriptionProducts
                          select new SubscriptionProductModel
                          {
                              SubscriptionId = pi.SubscriptionId,
                              ProductId = pi.ProductId,

                              SubscriptionTitle = db.Items.FirstOrDefault(i => i.Id.Equals(pi.SubscriptionId)).Title,
                              ProductTitle = db.Products.FirstOrDefault(i => i.Id.Equals(pi.ProductId)).Title
                          }).ToListAsync();
        }
        //Taking one subscriptionProduct and converting to one SubscriptionModel
        public static async Task<SubscriptionProductModel> Convert(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db,
            bool addListData = true)
        {
            var model = new SubscriptionProductModel
            {
                SubscriptionId = subscriptionProduct.SubscriptionId,
                ProductId = subscriptionProduct.ProductId,
                Subscriptions = addListData ? await db.Subscriptions.ToListAsync() : null,
                Products = addListData ? await db.Products.ToListAsync() : null,
                SubscriptionTitle = (await db.Items.FirstOrDefaultAsync(s => s.Id.Equals(subscriptionProduct.SubscriptionId))).Title,
                ProductTitle = (await db.Products.FirstOrDefaultAsync(p => p.Id.Equals(subscriptionProduct.ProductId))).Title
            };
            return model;
        }

        public static async Task<bool> CanChange(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var OldSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
            sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var NewSP = await db.SubscriptionProducts.CountAsync(sp => sp.ProductId.Equals(subscriptionProduct.ProductId) &&
            sp.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            return OldSP.Equals(1) && NewSP.Equals(0);
        }

        public static async Task Change(this SubscriptionProduct subscriptionProduct, ApplicationDbContext db)
        {
            var OldSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(sp => sp.ProductId.Equals(subscriptionProduct.OldProductId) &&
            sp.SubscriptionId.Equals(subscriptionProduct.OldSubscriptionId));

            var NewSubscriptionProduct = await db.SubscriptionProducts.FirstOrDefaultAsync(pi => pi.ProductId.Equals(subscriptionProduct.ProductId) &&
            pi.SubscriptionId.Equals(subscriptionProduct.SubscriptionId));

            if (OldSubscriptionProduct != null && NewSubscriptionProduct == null)
            {
                NewSubscriptionProduct = new SubscriptionProduct
                {
                    SubscriptionId = subscriptionProduct.SubscriptionId,
                    ProductId = subscriptionProduct.ProductId
                };

                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    db.SubscriptionProducts.Remove(OldSubscriptionProduct);
                    db.SubscriptionProducts.Add(NewSubscriptionProduct);

                    await db.SaveChangesAsync();
                    transaction.Complete();
                }
            }
        }
        #endregion
    }
}