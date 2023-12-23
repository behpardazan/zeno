using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using DataLayer.Base;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Data.OleDb;
using LinqToExcel;
using System.Data.Entity.Validation;

namespace Panel.Areas.Store.Controllers
{
    [ValidateInput(false)]
    public class ProductController : Controller
    {

        private UnitOfWork _context = new UnitOfWork();
        private UnitOfWork _unitContext = new UnitOfWork();

        public ActionResult Index(
            string shopId = null,
            string typeId = null,
            string categoryId = null,
            string subcategoryId = null,
            int? index = null,
            int? pagesize = null,
            string name = null,
            string active = "true",bool ? isSpecialProduct=null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;

            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Product);
            FillDropDowns(null);

            int? userId = (_context.UserRole.HasUserRole(_context.SiteUserUserGroup.GetAllBySiteUserId(CurrentUser.ID), CurrentUser, Enum_UserRole.ADMIN)) ? default(int?) : _context.SiteUser.GetCurrentUser().ID;
            List<Product> list = _context.Product.Search(pageSize: pagesize.GetValueOrDefault(), index: index.GetValueOrDefault(), typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, userId: userId, order: Enum_ProductOrder.NEW, activeLocation: false, isPublish : null, isSpecialProduct: isSpecialProduct);
            ViewBag.TotalCount = _context.Product.SearchDetail(typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, userId: userId, activeLocation: false,  isPublish : null, isSpecialProduct: isSpecialProduct).Count;
            return View(list.ToView());
        }
        public ActionResult QuantityProducts(
         string shopId = null,
         string typeId = null,
         string categoryId = null,
         string subcategoryId = null,
         int? index = null,
         int? pagesize = null,
         string name = null,
         string active = "true", bool? isSpecialProduct = null,string Id=null)
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;

            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Product);
            FillDropDowns(null);

            int? userId = (_context.UserRole.HasUserRole(_context.SiteUserUserGroup.GetAllBySiteUserId(CurrentUser.ID), CurrentUser, Enum_UserRole.ADMIN)) ? default(int?) : _context.SiteUser.GetCurrentUser().ID;
            List<Product> list = _context.Product.Search(Id:Id,pageSize: pagesize.GetValueOrDefault(), index: index.GetValueOrDefault(), typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, userId: userId, order: Enum_ProductOrder.NEW, activeLocation: false, isPublish: null, isSpecialProduct: isSpecialProduct);
            ViewBag.TotalCount = _context.Product.SearchDetail(Id: Id,typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, userId: userId, activeLocation: false, isPublish: null, isSpecialProduct: isSpecialProduct).Count;
            return View(list);
        }
        public ActionResult SearchAjax()
        {
            try
            {
                if (_context.Permission.HasPermission(Enum_Permission.Product))
                {
                    return new JsonResult()
                    {
                        Data = _context.Product.GetAll().Select(row => new { Id = row.ID, Name = row.Name }),
                        JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                        MaxJsonLength = int.MaxValue
                    };
                }
                else
                    return null;
            }
            catch (Exception ex)
            {
                return new JsonResult()
                {
                    Data = ex.Message,
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

       
        public ActionResult Details(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Details);
            Product product = _context.Product.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        public ActionResult Create(int? shopId)
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(CurrentUser, Enum_Permission.Product_New);

            List<Shop> listShops = _context.Shop.GetAllShopForUserId(CurrentUser.ID);

            shopId = shopId == null && listShops.Count > 0 ? listShops[0].ID : shopId;

            Product product = new Product()
            {
                ID = 0,
                Active = true,
                MinOrder = 1,
                LastPrice = 0,
                Price = 0,
                VisitCount = 0,
                ShowNumber = 0,
                Summary = "-",
                ShopId = shopId,
                ProductTypeId = 0,
                ProductCategoryId = 0,
                ProductSubCategoryId = 0,
                Latitude = "",
                Longitude = "",
                IsService = false,
                CreateDateTime = DateTime.Now


            };

            FillDropDowns(null);
            return View(product);
        }

        [HttpPost]
        public ActionResult Create(Product product)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_New);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            List<ProductCustomValue> CustomValues = product.ProductCustomValue.ToList();
            List<ProductPicture> ListPictures = product.ProductPicture.ToList();
            List<ProductColor> ListColors = product.ProductColor.ToList();
            List<ProductSize> ListSizes = product.ProductSize.ToList();
            List<ProductLanguage> listLanguage = product.ProductLanguage.ToList();
            List<Review> listReviw = product.Review.ToList();
            List<ProductVideo> listVideo = product.ProductVideo.ToList();
            List<ProductsType> listproductsType = product.ProductsType.ToList();
            List<ProductsCategory> listproductsCategory = product.ProductsCategory.ToList();
            List<ProductsSubCategory> listproductsSubCategory = product.ProductsSubCategory.ToList();
            product.ProductCustomValue.Clear();
            product.ProductPicture.Clear();
            product.ProductColor.Clear();
            product.ProductSize.Clear();
            product.ProductLanguage.Clear();
            product.Review.Clear();
            product.ProductVideo.Clear();
            product.ProductsType.Clear();
            product.ProductsCategory.Clear();
            product.ProductsSubCategory.Clear();
            if (IsModelValid(product, out error))
            {
                if (product.IsSpecialProduct == true)
                {
                    product.SpecialProduct = DateTime.Now;

                }
                product.VisitCount = 0;
                //product.IsAdvertising = null;

                product.UpdateDatetime = DateTime.Now;
                product.CreateDateTime = DateTime.Now;
                product.UserId = _context.SiteUser.GetCurrentUser().ID;
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
                product.CompanyId = product.CompanyId == 0 ? null : product.CompanyId;
                _context.Product.Insert(product);
                bool result = _context.Save();

                if (result == true)
                {
                    foreach (Review item in listReviw)
                    {
                        if (item.PictureId != null && item.Contect != "")
                        {
                            Review entity = new Review()
                            {
                                PictureId = item.PictureId,
                                Contect = item.Contect,
                                ProductId = product.ID
                            };
                            _context.Review.Insert(entity);
                        }

                    }
                    foreach (ProductVideo item in listVideo)
                    {
                       
                        ProductVideo entity = new ProductVideo()
                        {
                            PictureId = item.PictureId,
                            Url = item.Url,
                            ProductId = product.ID
                        };
                        _context.ProductVideo.Insert(entity);


                    }
                    foreach (ProductsType item in listproductsType)
                    {

                        ProductsType entity = new ProductsType()
                        {
                            ProductTypeId = item.ProductTypeId,
                            ProductId = product.ID
                        };
                        _context.ProductsType.Insert(entity);


                    }
                    foreach (ProductsCategory item in listproductsCategory)
                    {

                        ProductsCategory entity = new ProductsCategory()
                        {
                            ProductCategoryId = item.ProductCategoryId,
                            ProductId = product.ID
                        };
                        _context.ProductsCategory.Insert(entity);


                    }
                    foreach (ProductsSubCategory item in listproductsSubCategory)
                    {

                        ProductsSubCategory entity = new ProductsSubCategory()
                        {
                            ProductSubCategoryId = item.ProductSubCategoryId,
                            ProductId = product.ID
                        };
                        _context.ProductsSubCategory.Insert(entity);


                    }
                    foreach (ProductCustomValue item in CustomValues)
                    {
                        ProductCustomValue entity = new ProductCustomValue()
                        {
                            FieldId = item.FieldId,
                            ItemId = item.ItemId == 0 ? null : item.ItemId,
                            ProductId = product.ID,
                            Value = item.Value
                        };
                        
                        
                        _context.ProductCustomValue.Insert(entity);
                    }
                    foreach (ProductPicture item in ListPictures)
                    {
                        ProductPicture entity = new ProductPicture()
                        {
                            PictureId = item.PictureId,
                            ProductId = product.ID
                        };
                        _context.ProductPicture.Insert(entity);
                    }


                    foreach (ProductColor color in ListColors)
                    {
                        ProductColor entity = new ProductColor()
                        {
                            ProductId = product.ID,
                            ColorId = color.ColorId
                        };
                        _context.ProductColor.Insert(entity);
                    }
                    foreach (ProductSize size in ListSizes)
                    {
                        ProductSize entity = new ProductSize()
                        {
                            ProductId = product.ID,
                            SizeId = size.SizeId
                        };
                        _context.ProductSize.Insert(entity);
                    }
                    foreach (ProductLanguage item in listLanguage)
                    {
                        item.ProductId = product.ID;
                        _context.ProductLanguage.Insert(item);
                        _context.Save();
                    }
                    _context.Save();
                    if (product.IsCopy == true)
                    {
                        Product productCopy = new Product();
                        List<ProductCustomValue> CustomValuesCopy = CustomValues;
                        List<ProductPicture> ListPicturesCopy = ListPictures;
                        List<ProductColor> ListColorsCopy = ListColors;
                        List<ProductSize> ListSizesCopy = ListSizes;
                        List<ProductLanguage> listLanguageCopy = listLanguage;
                        List<Review> listReviwCopy = listReviw;
                        //productCopy = product;
                        //productCopy.ID = 0;
                        if (productCopy.IsSpecialProduct == true)
                        {
                            productCopy.SpecialProduct = DateTime.Now;
                            
                        }
                        productCopy.Name = product.Name;
                        productCopy.UpdateDatetime = DateTime.Now;
                        productCopy.CreateDateTime = DateTime.Now;
                        productCopy.UserId = _context.SiteUser.GetCurrentUser().ID;
                        productCopy.BrandId = product.BrandId == 0 ? null : product.BrandId;
                        productCopy.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                        productCopy.UnitId = product.UnitId == 0 ? null : product.UnitId;
                        productCopy.CompanyId = product.CompanyId == 0 ? null : product.CompanyId;
                        productCopy.Name = product.Name;
                        productCopy.ShopId = product.ShopId;
                        productCopy.ProductTypeId = product.ProductTypeId;
                        productCopy.ProductCategoryId = product.ProductCategoryId;
                        productCopy.ProductSubCategoryId = product.ProductSubCategoryId;
                        productCopy.StatusId = product.StatusId;
                        productCopy.Summary = product.Summary;
                        productCopy.CodeValue = product.CodeValue;
                        productCopy.ShowNumber = product.ShowNumber;
                        productCopy.Description = product.Description;
                        productCopy.Price = product.Price;
                        productCopy.ShowHomePage = product.ShowHomePage;
                        productCopy.PictureId = product.PictureId;
                        productCopy.DocId = product.DocId;
                        productCopy.Active = product.Active;
                        productCopy.SyncDatetime = product.SyncDatetime;
                        productCopy.VisitCount = product.VisitCount;
                        productCopy.IsService = product.IsService;
                        productCopy.H1 = product.H1;
                        productCopy.Title = product.Title;
                        productCopy.Url = product.Url;
                        productCopy.Img = product.Img;
                        productCopy.Latitude = product.Latitude;
                        productCopy.Longitude = product.Longitude;
                        productCopy.ProductOptionId = product.ProductOptionId;
                        productCopy.Weight = product.Weight;

                        _context.Product.Insert(productCopy);
                        bool result2 = _context.Save();

                        if (result2 == true)
                        {
                            foreach (Review item in listReviwCopy)
                            {
                                if (item.PictureId != null && item.Contect != "")
                                {
                                    Review entity = new Review()
                                    {
                                        PictureId = item.PictureId,
                                        Contect = item.Contect,
                                        ProductId = productCopy.ID
                                    };
                                    _context.Review.Insert(entity);
                                }

                            }
                            foreach (ProductCustomValue item in CustomValuesCopy)
                            {
                                ProductCustomValue entity = new ProductCustomValue()
                                {
                                    FieldId = item.FieldId,
                                    ItemId = item.ItemId == 0 ? null : item.ItemId,
                                    ProductId = productCopy.ID,
                                    Value = item.Value
                                };
                                _context.ProductCustomValue.Insert(entity);
                            }
                            foreach (ProductPicture item in ListPicturesCopy)
                            {
                                ProductPicture entity = new ProductPicture()
                                {
                                    PictureId = item.PictureId,
                                    ProductId = productCopy.ID
                                };
                                _context.ProductPicture.Insert(entity);
                            }


                            foreach (ProductColor color in ListColorsCopy)
                            {
                                ProductColor entity = new ProductColor()
                                {
                                    ProductId = productCopy.ID,
                                    ColorId = color.ColorId
                                };
                                _context.ProductColor.Insert(entity);
                            }
                            foreach (ProductSize size in ListSizesCopy)
                            {
                                ProductSize entity = new ProductSize()
                                {
                                    ProductId = productCopy.ID,
                                    SizeId = size.SizeId
                                };
                                _context.ProductSize.Insert(entity);
                            }
                            foreach (ProductLanguage item in listLanguageCopy)
                            {
                                item.ProductId = productCopy.ID;
                                _context.ProductLanguage.Insert(item);
                                _context.Save();
                            }
                            _context.Save();
                        }
                        _context.ProductQuantity.UpsertProductQuantiy(productCopy.ID, null, true, _context);
                    }
                    _context.ProductQuantity.UpsertProductQuantiy(product.ID, null, true, _context);

                    if (BaseWebsite.WebsiteSetting.HasUnitPrice)
                    {
                        double computeprice = _context.Product.ComputeFee();
                        _context.Product.UpdateProductPriceBaseUnitPrice(product.ID, computeprice, _context);
                    }



                }
            }
            return new JsonResult() { Data = error };
        }


        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();

            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            List<Shop> listShops = _context.Shop.GetAllShopForUserId(CurrentUser.ID);

            product.ShopId = product.ShopId == null && listShops.Count > 0 ? listShops[0].ID : product.ShopId;

            FillDropDowns(product);
            ViewBag.Message = BaseMessage.GetMessage();
            return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            SiteUser currentUser = _context.SiteUser.GetCurrentUser();
            _context.Permission.CheckPagePermission(currentUser, Enum_Permission.Product_Edit);

            List<ProductCustomValue> CustomValues = product.ProductCustomValue.ToList();
            List<ProductPicture> ListPictures = product.ProductPicture.ToList();
            List<ProductColor> ListColors = product.ProductColor.ToList();
            List<ProductSize> ListSizes = product.ProductSize.ToList();
            List<ProductLanguage> listLanguage = product.ProductLanguage.ToList();
            List<ProductVideo> listVideo = product.ProductVideo.ToList();
            List<ProductsType> listproductsType = product.ProductsType.ToList();
            List<ProductsCategory> listproductsCategory = product.ProductsCategory.ToList();
            List<ProductsSubCategory> listproductsSubCategory = product.ProductsSubCategory.ToList();
            List<Review> listReview = product.Review.ToList();
            product.ProductCustomValue.Clear();
            product.ProductVideo.Clear();

            product.ProductPicture.Clear();
            var LastListSizes = _context.ProductSize.Where(s => s.ProductId == product.ID).ToList();
            var LastListColors = _context.ProductColor.Where(s => s.ProductId == product.ID).ToList();

            product.ProductColor.Clear();
            product.ProductSize.Clear();
            product.ProductLanguage.Clear();
            product.Review.Clear();
            product.ProductsType.Clear();
            product.ProductsCategory.Clear();
            product.ProductsSubCategory.Clear();
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            if (IsModelValid(product, out error))
            {
                if (product.IsSpecialProduct == true)
                {
                    product.SpecialProduct = DateTime.Now;

                }
                //product.IsAdvertising = null;
                product.UpdateDatetime = DateTime.Now;
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
                product.CompanyId = product.CompanyId == 0 ? null : product.CompanyId;
                product.UserId = currentUser.ID;
                product.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                _context.Product.Update(product);
                _context.Save();
                _context.ProductCustomValue.DeleteByProductId(product.ID);
                _context.ProductPicture.DeleteByProductId(product.ID, true);
                _context.ProductColor.DeleteByProductId(product.ID);
                _context.ProductSize.DeleteByProductId(product.ID);
                _context.ProductLanguage.DeleteByProductId(product.ID);
                _context.Review.DeleteByProductId(product.ID);
                _context.ProductVideo.DeleteByProductId(product.ID);
                _context.ProductsCategory.DeleteByProductId(product.ID);
                _context.ProductsType.DeleteByProductId(product.ID);
                _context.ProductsSubCategory.DeleteByProductId(product.ID);
                bool result = _context.Save();
                foreach (ProductCustomValue item in CustomValues)
                {
                    ProductCustomValue entity = new ProductCustomValue()
                    {
                        FieldId = item.FieldId,
                        ItemId = item.ItemId == 0 ? null : item.ItemId,
                        ProductId = product.ID,
                        Value = item.Value
                    };
                    //var customField = _unitContext.ProductCustomField.GetById(item.FieldId);
                    //if (customField.SyncName == "SpecialProduct")
                    //{
                    //    product.SpecialProduct = DateTime.Now;
                    //    _context.Product.Update(product);
                    //    _context.Save();
                    //}
                    _context.ProductCustomValue.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductPicture item in ListPictures)
                {
                    ProductPicture entity = new ProductPicture()
                    {
                        PictureId = item.PictureId,
                        ProductId = product.ID
                    };
                    _context.ProductPicture.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductVideo item in listVideo)
                {
                    ProductVideo entity = new ProductVideo()
                    {
                        Url = item.Url,
                        PictureId = item.PictureId,
                        ProductId = product.ID,
                      
                    };
                    _context.ProductVideo.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductColor color in ListColors)
                {
                    ProductColor entity = new ProductColor()
                    {
                        ProductId = product.ID,
                        ColorId = color.ColorId
                    };
                    _context.ProductColor.Insert(entity);
                }

                foreach (ProductSize size in ListSizes)
                {
                    ProductSize entity = new ProductSize()
                    {
                        ProductId = product.ID,
                        SizeId = size.SizeId
                    };
                    _context.ProductSize.Insert(entity);
                }
                result = _context.Save();
                foreach (ProductLanguage item in listLanguage)
                {
                    item.ProductId = product.ID;
                    _context.ProductLanguage.Insert(item);
                    _context.Save();
                }
                result = _context.Save();
                foreach (ProductsType item in listproductsType)
                {
                    ProductsType entity = new ProductsType()
                    {

                        ProductId = product.ID,
                        ProductTypeId = item.ProductTypeId
                    };
                    _context.ProductsType.Insert(entity);
                }

                result = _context.Save();
                foreach (ProductsCategory item in listproductsCategory)
                {
                    ProductsCategory entity = new ProductsCategory()
                    {

                        ProductId = product.ID,
                        ProductCategoryId = item.ProductCategoryId
                    };
                    _context.ProductsCategory.Insert(entity);
                }

                result = _context.Save();
                foreach (ProductsSubCategory item in listproductsSubCategory)
                {
                    ProductsSubCategory entity = new ProductsSubCategory()
                    {

                        ProductId = product.ID,
                        ProductSubCategoryId = item.ProductSubCategoryId
                    };
                    _context.ProductsSubCategory.Insert(entity);
                }

                result = _context.Save();
                foreach (Review review in listReview)
                {
                    if (review.PictureId != null && review.Contect != "")
                    {
                        Review entity = new Review()
                        {
                            PictureId = review.PictureId,
                            Contect = review.Contect,
                            ProductId = product.ID
                        };
                        _context.Review.Insert(entity);
                    }
                }
                result = _context.Save();
                if (BaseWebsite.WebsiteSetting.HasUnitPrice)
                {
                    double computeprice = _context.Product.ComputeFee();

                    _context.Product.UpdateProductPriceBaseUnitPrice(product.ID, computeprice, _context);

                }
                else
                {
                    _context.ProductQuantity.UpsertProductQuantiy(product.ID, null, false, _context);
                }
            }
            return new JsonResult() { Data = error };
        }
        public ActionResult Quantity(int? id)
        {

            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            ViewBag.Product = _context.Product.GetById(id.GetValueOrDefault());

            List<ProductQuantity> listQuantity = _context.ProductQuantity.GetAllByProductId(id.GetValueOrDefault());
            return View(listQuantity);
        }

        [HttpPost]
        public ActionResult Quantity(List<ProductQuantity> listQuantiy, int productId)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            _context.ProductQuantity.UpdateAll(listQuantiy, productId);


            return new JsonResult() { Data = error };
        }

        private bool IsModelValid(Product product, out ViewMessage msg)
        {
            bool result = false;

            product.DocId = product.DocId != 0 ? product.DocId : null;
            product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            product.ProductCategoryId = product.ProductCategoryId != 0 ? product.ProductCategoryId : null;
            product.ProductSubCategoryId = product.ProductSubCategoryId != 0 ? product.ProductSubCategoryId : null;

            if (product.ShopId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            else if (product.Name == null)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            else if (product.StatusId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            //else if (product.Name.Length > 40)
            //    msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            else
            {
                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;
            }

            return result;
        }

        private void FillDropDowns(Product product)
        {
            SiteUser CurrentUser = _context.SiteUser.GetCurrentUser();
            ViewBag.BrandId = new SelectList(new List<ProductBrand>(), "ID", "Name");
            ViewBag.ProductOptionId = new SelectList(new List<ProductOption>(), "ID", "Name");
            ViewBag.ProductTypeId = new SelectList(new List<ProductType>(), "ID", "Name");
            ViewBag.ShopId = new SelectList(_context.Shop.GetAllShopForUserId(CurrentUser.ID), "ID", "Name", product != null ? product.ShopId : 0);
            ViewBag.ProductCategoryId = new SelectList(new List<ProductCategory>(), "ID", "Name", 0);
            ViewBag.ProductSubCategoryId = new SelectList(new List<ProductSubCategory>(), "ID", "Name", product != null ? product.ProductSubCategoryId : 0);
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name", product != null ? product.StatusId : 0);
            ViewBag.UnitId = new SelectList(_context.Unit.GetAll(), "ID", "Name", product != null ? product.UnitId : 0);
            ViewBag.CompanyId = new SelectList(_context.ProductCompany.GetAll(), "ID", "Name", product != null ? product.CompanyId : 0);

        }

        [HttpGet]
        public ActionResult FillProductType(int? ShopId)
        {
            return new JsonResult()
            {
                Data = _context.ProductType.Search(shopId: ShopId, pageSize: 100).ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult FillState(int? ShopId)
        {
            return new JsonResult()
            {
                Data = _context.State.Where(s => s.CountryId == 118 && s.ID != 0).OrderBy(s => s.Name).ToList().ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult FillCity(int stateId)
        {
            return new JsonResult()
            {
                Data = _context.City.Where(s => s.StateId == stateId && s.ID != 0).OrderBy(s => s.Name).ToList().ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult FillProductCategory(string TypeId)
        {
            List<ProductCategory> list = new List<ProductCategory>();
            string[] Ids = TypeId.Split(',');
            foreach (var item in Ids)
            {
                if (item != "")
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        var categoryList = _context.ProductCategory.Search(typeId: itemId, pageSize: 100);
                        foreach (var category in categoryList)
                        {
                            list.Add(category);
                        }
                    }
                }

            }

            return new JsonResult()
            {
                Data = list.ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductSubCategory(string CategoryId)
        {
            List<ProductSubCategory> list = new List<ProductSubCategory>();
            string[] Ids = CategoryId.Split(',');
            foreach (var item in Ids)
            {
                if (item != "")
                {
                    int itemId = item.GetInteger();
                    if (itemId != 0)
                    {
                        var categoryList = _context.ProductSubCategory.Search(categoryId: CategoryId, pageSize: 100);
                        foreach (var category in categoryList)
                        {
                            list.Add(category);
                        }
                    }
                }

            }

            return new JsonResult()
            {
                Data = list.ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        [HttpGet]
        public ActionResult FillProductBrand(string TypeId)
        {
            List<ProductSubCategory> list = new List<ProductSubCategory>();
            var setting = BaseWebsite.WebsiteSetting;
            int typeId;
            if (setting.HasMultiCategory == true)
            {
                string[] Ids = TypeId.Split(',');
                typeId = int.Parse(Ids[0]);
            }
            else {
                typeId = int.Parse(TypeId);


            }
            SiteUser user = _context.SiteUser.GetCurrentUser();
            return new JsonResult()
            {
                Data = _context.ProductBrand.Search(typeId: typeId, pageSize: 2000, userId: user.ID).ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductCustomFields(string TypeId = "0", string CategoryId = "0", string SubCategoryId = "0", int BrandId = 0, int ProductId = 0,bool ?noSpical=false)
        {
            var setting = BaseWebsite.WebsiteSetting;
            int typeId;int categoryId;int subCategoryId;
            if (setting.HasMultiCategory == true)
            {
                string[] Ids = TypeId.Split(',');
                string[] Ids2 = CategoryId.Split(',');
                string[] Ids3 = SubCategoryId.Split(',');

                typeId = int.Parse(Ids[0]);
                categoryId = int.Parse(Ids2[0]);
                subCategoryId = int.Parse(Ids3[0]);

            }
            else
            {
                typeId = int.Parse(TypeId);
                categoryId = int.Parse(CategoryId);
                subCategoryId = int.Parse(SubCategoryId);


            }
            List<ProductCustomField> list = _context.ProductCustomField.Search(typeId: typeId, categoryId: categoryId, subcategoryId: subCategoryId, brandId: BrandId, pageSize: 1000);
            if (noSpical == true)
            {
                list = list.Where(s => s.SyncName != "SpecialProduct").ToList();
            }
            List<ViewProductCustomField> listFields = list.ToView();
            if (ProductId != 0)
            {
                for (int i = 0; i < list.Count; i++)
                {
                    ProductCustomField item = list[i];
                    ProductCustomValue value = item.ProductCustomValue.FirstOrDefault(p => p.ProductId == ProductId && p.FieldId == item.ID);
                    if (value != null)
                    {
                        if (item.Code.Label == Enum_Code.FIELD_TYPE_DROPDOWN.ToString())
                            listFields[i].ProductFieldItem = value.ItemId;
                        else
                            listFields[i].ProductFieldValue = value.Value;
                    }
                }
            }
           
            return new JsonResult()
            {
                Data = listFields,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillColors(string TypeId, int ProductId)
        {
            var setting = BaseWebsite.WebsiteSetting;
            int typeId;
            if (setting.HasMultiCategory == true)
            {
                string[] Ids = TypeId.Split(',');
                typeId = int.Parse(Ids[0]);
            }
            else
            {
                typeId = int.Parse(TypeId);


            }
            List<ViewColor> listColor = _context.Color.GetAllProductTypeId(typeId).ToView();
            List<ProductColor> listSelectedColor = _context.ProductColor.GetAllByProductId(ProductId);
            foreach (ViewColor color in listColor)
            {
                color.IsSelected = listSelectedColor.Any(p => p.ColorId == color.Id);
            }

            return new JsonResult()
            {
                Data = listColor,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillSize(string TypeId, int ProductId)
        {
            var setting = BaseWebsite.WebsiteSetting;
            int typeId;
            if (setting.HasMultiCategory == true)
            {
                string[] Ids = TypeId.Split(',');
                typeId = int.Parse(Ids[0]);
            }
            else
            {
                typeId = int.Parse(TypeId);


            }
            List<ViewSize> listSize = _context.Size.GetAllProductTypeId(typeId).ToView();
            List<ProductSize> listSelectedSize = _context.ProductSize.GetAllByProductId(ProductId);
            foreach (ViewSize size in listSize)
            {
                size.IsSelected = listSelectedSize.Any(p => p.SizeId == size.Id);
            }

            return new JsonResult()
            {
                Data = listSize,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult FillProductOption(string  typeId=null)
        {
            var setting = BaseWebsite.WebsiteSetting;
            int ? TypeId=null;
            if (setting.HasMultiCategory == true)
            {
                string[] Ids = typeId.Split(',');
                TypeId = int.Parse(Ids[0]);
            }
            else
            {
                TypeId = int.Parse(typeId);


            }
            var listOption = _context.ProductOption.Search(TypeId, 200, 1).ToView();
            return new JsonResult()
            {
                Data = listOption,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();

            return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Delete);
            //Product product = _context.Product.GetById(id);

            //_context.ProductPicture.DeleteByProductId(product.ID);
            //_context.ProductLanguage.DeleteByProductId(product.ID);
            //_context.ProductLike.DeleteByProductId(product.ID);
            //_context.Discount.DeleteByProductId(product.ID);
            //_context.ProductCustomValue.DeleteByProductId(product.ID);
            //_context.ProductColor.DeleteByProductId(product.ID);
            //_context.ProductSize.DeleteByProductId(product.ID);
            //_context.ProductQuantity.DeleteByProductId(product.ID);
            //_context.Save();

            _context.Product.Hide(id);
            _context.Save();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Picture(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();
            return View(product);
        }

        [HttpPost]
        public ActionResult Picture([Bind(Include = "ID")] Product product, HttpPostedFileBase file, int? ColorId = null)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Product_Edit);

            if (file != null)
            {
                Picture picture = _context.Picture.CreateAndUpload(Enum_Code.SYSTEM_TYPE_PANEL, file);
                ProductPicture pic = new ProductPicture()
                {
                    PictureId = picture.ID,
                    ProductId = product.ID,
                    ColorId = ColorId
                };
                _context.ProductPicture.Insert(pic);
                _context.Save();
            }
            //product = _context.Product.GetById(product.ID);
            return RedirectToAction("picture", new { id = product.ID });
        }

        [HttpPost]
        public ActionResult PictureDelete(int ProductId, int PictureId)
        {
            _context.ProductPicture.Delete(PictureId);
            _context.Save();
            return RedirectToAction("Picture", "Product", new { id = ProductId });
        }
        //public void AddId()
        //{
        //    var listProduct = _context.Product.GetAll().ToList();
        //    foreach (var item in listProduct)
        //    {
        //        try
        //        {
        //            UnitOfWork _context = new UnitOfWork();
        //            if (item.ProductTypeId != null)
        //            {
        //                ProductsType type = new ProductsType();
        //                type.ProductId = item.ID;
        //                type.ProductTypeId = item.ProductTypeId;
        //                _context.ProductsType.Insert(type);
        //                _context.Save();
        //            }
        //        }
        //        catch
        //        {
        //            _context.Dispose();
        //        }
        //        try
        //        {
        //            UnitOfWork _context = new UnitOfWork();
        //            if (item.ProductCategoryId != null)
        //            {
        //                ProductsCategory catgory = new ProductsCategory();
        //                catgory.ProductId = item.ID;
        //                catgory.ProductCategoryId = item.ProductCategoryId;
        //                _context.ProductsCategory.Insert(catgory);
        //                _context.Save();
        //            }
        //        }
        //        catch
        //        {
        //            _context.Dispose();
        //        }
        //        try
        //        {
        //            UnitOfWork _context = new UnitOfWork();
        //            if (item.ProductSubCategoryId != null && item.ProductSubCategoryId != 0)
        //            {
        //                ProductsSubCategory subcatgory = new ProductsSubCategory();
        //                subcatgory.ProductId = item.ID;
        //                subcatgory.ProductSubCategoryId = item.ProductSubCategoryId;
        //                _context.ProductsSubCategory.Insert(subcatgory);
        //                _context.Save();
        //            }
        //        }
        //        catch
        //        {
        //            _context.Dispose();
        //        }
        //    }

        //}

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _context.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}