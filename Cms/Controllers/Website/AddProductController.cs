using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DataLayer.Entities;
using DataLayer.Base;
using Cms.Areas.Store.Security;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System.Text;
using DataLayer.Helpers.Common;

namespace Cms.Controllers.Website
{
    public class AddProductController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        private UnitOfWork _Unitcontext = new UnitOfWork();

        public ActionResult Index(
            string shopId = null,
            string typeId = null,
            string categoryId = null,
            string subcategoryId = null,
            int? index = null,
            int? pagesize = null,
            string name = null,
            string active = null
            )
        {
            index = index == null ? 1 : index;
            pagesize = pagesize == null ? 10 : pagesize;
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });

            FillDropDowns(null);

          
            List<Product> list = _context.Product.Search(pageSize: pagesize.GetValueOrDefault(), index: index.GetValueOrDefault(), typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, order: Enum_ProductOrder.NEW, activeLocation: false, accountId:account.Id, isAdvertising:true,isPublish:null);
            ViewBag.TotalCount = _context.Product.SearchDetail(typeId: typeId, shopId: null, categoryId: categoryId, subCategoryId: subcategoryId, name: name, active: active, activeLocation: false, accountId: account.Id, isAdvertising:true, isPublish: null).Count;
            return PartialView(BaseController.GetView(this), list.ToView());

        }
        public ActionResult SearchAjax()
        {
            try
            {

                return new JsonResult()
                {
                    Data = _context.Product.GetAll().Select(row => new { Id = row.ID, Name = row.Name }),
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet,
                    MaxJsonLength = int.MaxValue
                };

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
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


            Product product = _context.Product.GetById(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }
        //[AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        //public ActionResult LadderPaymentStatus(int? productId)
        //{
        //    ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
        //    if (productId == null)
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);


        //    LadderPayment payment = _context.LadderPayment.Where(s => s.ProductId == productId && s.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString() && s.AccountId == CurrentUser.Id).LastOrDefault();
        //    if (payment == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return PartialView(BaseController.GetView(this), payment);
        //}
        public ActionResult Create(int? shopId)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });

            shopId = null;

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
                Name = "-",
                Description = "-",
                ShopId = shopId,
                ProductTypeId = 0,
                ProductCategoryId = 0,
                ProductSubCategoryId = 0,
                Latitude = "",
                Longitude = "",

                IsService = false


            };

            FillDropDowns(null);
            return PartialView(BaseController.GetView(this), product);
            //return View(product);
        }
        [HttpPost]
        public ActionResult Create(Product product)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            List<ProductCustomValue> CustomValues = product.ProductCustomValue.ToList();
            List<ProductPicture> ListPictures = product.ProductPicture.ToList();
            List<ProductColor> ListColors = product.ProductColor.ToList();
            List<ProductSize> ListSizes = product.ProductSize.ToList();
            List<ProductLanguage> listLanguage = product.ProductLanguage.ToList();
            List<Review> listReviw = product.Review.ToList();
            List<ProductVideo> listProductVideo = product.ProductVideo.ToList();
            string currentLang = BaseLanguage.GetCurrentLanguage().ToString();
            product.ProductCustomValue.Clear();
            product.ProductPicture.Clear();
            product.ProductColor.Clear();
            product.ProductSize.Clear();
            product.ProductVideo.Clear();

            product.Review.Clear();
            if (IsModelValid(product, out error))
            {

                product.ProductLanguage.Clear();
                product.IsAdvertising = true;
                product.IsPublish = null;
                product.VisitCount = 0;
                product.CreateDateTime = DateTime.Now;
                product.UserId = null;
                product.Active = false;
                product.MinPrice = product.Price!=null? product.Price .Value: 0;
                product.LastPrice = product.Price != null ? product.Price.Value : 0;
                product.AccountId = account.Id;
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.ProductOptionId = product.ProductOptionId == 0 ? null : product.ProductOptionId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
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
                   

                    StringBuilder str = new StringBuilder();
                    ViewWebsite website = BaseWebsite.ShopWebsite;

                    var mobile = account.Mobile;


                    string token1 = "https://khoshkala.com/addproduct";
                    string token2 = "";
                    string token3 = "";
                    str.AppendLine("آگهی شما در سایت ثبت و در حال بررسی است");

                    _context.Sms.SaveNewSms(mobile, Enum_SmsType.SUBMIT_ADDS, str.ToString(), token1, token2, token3);
                   

                }
            }
           
            return new JsonResult() { Data = error };
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();
          
            product.ShopId =  null ;
            FillDropDowns(product);
            ViewBag.Message = BaseMessage.GetMessage();
            return PartialView(BaseController.GetView(this), product);

            //return View(product);
        }
        [HttpPost]
        public ActionResult Edit(Product product)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });

            List<ProductCustomValue> CustomValues = product.ProductCustomValue.ToList();
            List<ProductPicture> ListPictures = product.ProductPicture.ToList();
            List<ProductColor> ListColors = product.ProductColor.ToList();
            List<ProductSize> ListSizes = product.ProductSize.ToList();
            List<ProductLanguage> listLanguage = product.ProductLanguage.ToList();
            List<Review> listReview = product.Review.ToList();
            List<ProductVideo> listVideo = product.ProductVideo.ToList();
            string currentLang = BaseLanguage.GetCurrentLanguage().ToString();

            product.ProductCustomValue.Clear();
            product.ProductPicture.Clear();
            product.ProductVideo.Clear();

            var LastListSizes = _context.ProductSize.Where(s => s.ProductId == product.ID).ToList();
            var LastListColors = _context.ProductColor.Where(s => s.ProductId == product.ID).ToList();

            product.ProductColor.Clear();
            product.ProductSize.Clear();
            //var account = _context.Account.GetCurrentAccount();
            //product.AccountId = account.Id;
            product.Review.Clear();
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };

            if (IsModelValid(product, out error))
            {

                var tem = _Unitcontext.Product.GetById(product.ID);
                product.CreateDateTime = tem.CreateDateTime;
                product.ProductLanguage.Clear();
              
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
                product.UserId = null;
                product.IsAdvertising = true;
                product.Active = false;
                product.IsPublish = null;
                product.MinPrice = product.Price != null ? product.Price.Value : 0;
                product.LastPrice = product.Price != null ? product.Price.Value : 0;

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
                foreach (ProductColor color in ListColors)
                {
                    ProductColor entity = new ProductColor()
                    {
                        ProductId = product.ID,
                        ColorId = color.ColorId
                    };
                    _context.ProductColor.Insert(entity);
                }
                result = _context.Save();
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
                //try
                //{
                //    var currentAccount = DataLayer.Base.BaseWebsite.CurrentAccount;
                //    var model = _context.StoreProduct.Where(s => s.ProductId == product.ID && s.StoreId == currentAccount.StoreId).FirstOrDefault();
                //    var listQuantity = _context.StoreProductQuantity.Where(s => s.StoreProduct.ProductId == product.ID);
                //    foreach (var item in listQuantity)
                //    {
                //        _context.StoreProductQuantity.Delete(item);

                //    }
                //    result = _context.Save();

                //    _context.ProductQuantity.UpsertProductQuantiy(product.ID, product.Price, false, _context);
                //    foreach (var quantity in product.ProductQuantity)
                //    {
                //        StoreProductQuantity productQuantity = new StoreProductQuantity();

                //        productQuantity.Price = product.Price != null ? product.Price.Value : 0;
                //        productQuantity.Count = 1;
                //        productQuantity.ProductQuantityId = quantity.ID;
                //        productQuantity.StoreProductId = model.ID;
                //        _context.StoreProductQuantity.AddOrEdit(productQuantity, _context);
                //    }
                //    result = _context.Save();

                //    result = _context.Save();

                //}
                //catch (Exception ex)
                //{
                //    return new JsonResult() { Data = error };
                //}


                //result = _context.Save();

            }
            return new JsonResult() { Data = error };
        }

        private bool IsModelValid(Product product, out ViewMessage msg)
        {
            bool result = false;
            int find = 0;
            product.DocId = product.DocId != 0 ? product.DocId : null;
            product.PictureId = product.PictureId != 0 ? product.PictureId : null;
            product.ProductTypeId = product.ProductTypeId != 0 ? product.ProductTypeId : null;
            product.ProductCategoryId = product.ProductCategoryId != 0 ? product.ProductCategoryId : null;
            product.ProductSubCategoryId = product.ProductSubCategoryId != 0 ? product.ProductSubCategoryId : null;
            if (product.ShopId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SHOP);
            else if (product.Name == null || product.Name == "")
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_NAME);
            else if (product.Name.Length > 45)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PRODUCT_NAME);
            else if (product.Summary == null || product.Summary == "")
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_SUMMARY);
            else if (string.IsNullOrEmpty(product.Phone))
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PHONE);
            else if (product.ProductTypeId==0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTTYPE);
            else if (product.ProductCategoryId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTCATEGORY);
            else if (product.ProductSubCategoryId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTSUBCATEGORY);
            else if (product.BrandId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTBRAND);
            else if (product.Phone.Length>11 || product.Phone.Length< 11)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.INVALID_PRODUCT_PHONE);
            else if (product.StatusId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_STATUS);
            else if (product.CityId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_ACCOUNT_City_VALUE);
            else if (product.StateId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_ACCOUNT_State_VALUE);
            else
            {

                msg = BaseMessage.GetMessage(Enum_MessageType.SUCCESS);
                result = true;


            }

            return result;
        }

        private void FillDropDowns(Product product)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            ViewBag.BrandId = new SelectList(new List<ProductBrand>(), "ID", "Name");
            ViewBag.ProductOptionId = new SelectList(new List<ProductOption>(), "ID", "Name");
            ViewBag.ProductTypeId = new SelectList(new List<ProductType>(), "ID", "Name");
            ViewBag.StateId = new SelectList(new List<State>(), "ID", "Name");
            ViewBag.CityId = new SelectList(new List<City>(), "ID", "Name");
            ViewBag.ShopId = new SelectList(_context.Shop.GetAllShopForUserId(CurrentUser.Id), "ID", "Name", product != null ? product.ShopId : 0);
            ViewBag.ProductCategoryId = new SelectList(new List<ProductCategory>(), "ID", "Name", 0);
            ViewBag.ProductSubCategoryId = new SelectList(new List<ProductSubCategory>(), "ID", "Name", product != null ? product.ProductSubCategoryId : 0);
            ViewBag.StatusId = new SelectList(_context.Code.GetAllByCodeGroup(Enum_CodeGroup.PRODUCT_STATUS), "ID", "Name", product != null ? product.StatusId : 0);
            ViewBag.UnitId = new SelectList(_context.Unit.GetAll(), "ID", "Name", product != null ? product.UnitId : 0);

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
        public ActionResult FillProductCategory(int TypeId)
        {

            return new JsonResult()
            {
                Data = _context.ProductCategory.Search(typeId: TypeId, pageSize: 100).ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductSubCategory(string CategoryId = null)
        {
            List<ViewProductSubCategory> listSubCategory = (CategoryId == null) ? new List<ViewProductSubCategory>() : _context.ProductSubCategory.Search(categoryId: CategoryId, pageSize: 100).ToView();
            return new JsonResult()
            {
                Data = listSubCategory,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductBrand(int TypeId)
        {
            ViewAccount CurrentUser = _context.Account.GetCurrentAccount();
            return new JsonResult()
            {
                Data = _context.ProductBrand.Search(typeId: TypeId, pageSize: 2000, userId: CurrentUser.Id).ToView(),
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }

        [HttpGet]
        public ActionResult FillProductCustomFields(int TypeId = 0, int CategoryId = 0, int SubCategoryId = 0, int BrandId = 0, int ProductId = 0)
        {
            List<ProductCustomField> list = _context.ProductCustomField.Search(typeId: TypeId, categoryId: CategoryId, subcategoryId: SubCategoryId, brandId: BrandId, pageSize: 1000);
            List<ViewProductCustomField> listFields = list.Where(s => s.Active != false).ToList().ToView();
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
        public ActionResult FillColors(int TypeId, int ProductId)
        {
            List<ViewColor> listColor = _context.Color.GetAllProductTypeId(TypeId).ToView();
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
        public ActionResult FillSize(int TypeId, int ProductId)
        {
            List<ViewSize> listSize = _context.Size.GetAllProductTypeId(TypeId).ToView();
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
        public ActionResult FillProductOption(int? typeId)
        {

            var listOption = _context.ProductOption.Search(typeId, 200, 1).ToView();
            return new JsonResult()
            {
                Data = listOption,
                JsonRequestBehavior = JsonRequestBehavior.AllowGet
            };
        }
        public ActionResult Delete(int? id)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Product product = _context.Product.GetById(id);
            if (product == null)
                return HttpNotFound();
            return PartialView(BaseController.GetView(this), product);
            //return View(product);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            if (account == null)
                return RedirectToAction("login", new { controller = "account" });
            _context.Product.Hide(id);
            _context.Save();
            return RedirectToAction("Index");
        }
        //[AuthorizeFilter(Enum_StorePanel = DataLayer.Enumarables.Enum_StorePanel.StoreState)]
        //public ActionResult Sold(int id)
        //{
        //    Product product = _context.Product.GetById(id);
        //    product.IsSales = true;
        //    _context.Product.Update(product);
        //    _context.Save();
        //    return RedirectToAction("Index");
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
