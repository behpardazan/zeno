using DataLayer.Api;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using Quartz;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLayer.Schedules
{
    public class Sync_Discount : IJob
    {
        public void Execute(IJobExecutionContext context)
        {
            UnitOfWork _context = new UnitOfWork();
            var discount = _context.Discount.Where(s=>s.EndDatetime<=DateTime.Now).ToList();
            if (Sync_Base.Is_Discount_Syncing == false)
            {
                Sync_Base.Is_Discount_Syncing = true;
                try
                {
                    SyncLog log = new SyncLog()
                    {
                        Name = "Discount",
                        StartDatetime = DateTime.Now
                    };


                    foreach (var item in discount)
                    {
                        if (item.ProductId != null)
                        {
                            DataLayer.Base.BaseStore.UpdateProductQuantity(productId: item.ProductId.Value, _context);
                        }
                        else if (item.ProductTypeId != null)
                        {
                            var products = item.ProductType.Product.ToList();
                            foreach (var product in products)
                            {
                                DataLayer.Base.BaseStore.UpdateProductQuantity(productId: product.ID, _context);

                            }
                        }
                        else if (item.ProductCategoryId != null)
                        {
                            var products = item.ProductCategory.Product.ToList();
                            foreach (var product in products)
                            {
                                DataLayer.Base.BaseStore.UpdateProductQuantity(productId: product.ID, _context);

                            }
                        }
                        else if (item.ProductSubCategoryId != null)
                        {
                            var products = item.ProductSubCategory.Product.ToList();
                            foreach (var product in products)
                            {
                                DataLayer.Base.BaseStore.UpdateProductQuantity(productId: product.ID, _context);

                            }
                        }
                        else if (item.StoreProductQuantityId != null)
                        {
                            var product = item.StoreProductQuantity.StoreProduct.Product;

                            DataLayer.Base.BaseStore.UpdateProductQuantity(productId: product.ID, _context);


                        }
                        else if (item.ProductBrandId != null)
                        {
                            var products = item.ProductBrand.Product.ToList();
                            foreach (var product in products)
                            {
                                DataLayer.Base.BaseStore.UpdateProductQuantity(productId: product.ID, _context);

                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    string a = ex.Message;
                }
                Sync_Base.Is_Discount_Syncing = false;
            }
        }
    }
}
