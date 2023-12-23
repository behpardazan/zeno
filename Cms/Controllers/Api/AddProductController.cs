using DataLayer.Api;
using DataLayer.Api.Model;
using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace Cms.Controllers.Api
{
    public class AddProductController : ApiController
    {
        UnitOfWork _context = new UnitOfWork();
        private UnitOfWork _Unitcontext = new UnitOfWork();

        [HttpPost]
        public ApiResult Post(Product product)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
           
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            ApiResult resultApi = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Success,
                Message = error.Body
            };
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
                product.MinPrice = product.Price != null ? product.Price.Value : 0;

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
            else
            {
                resultApi.Code = ApiResult.ResponseCode.Error;
                resultApi.Message = error.Body;
            }
           
            //result.Value = obj.ToJson();
            return resultApi;

        }
        [HttpPut]
        public ApiResult Put(Product product)
        {
            ViewAccount account = _context.Account.GetCurrentAccount();
            //if (account == null)
            //    return RedirectToAction("login", new { controller = "account" });
            ViewMessage error = new ViewMessage() { Type = Enum_MessageType.SUCCESS };
            ApiResult resultApi = new ApiResult()
            {
                Code = ApiResult.ResponseCode.Success,
                Message = error.Body
            };
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

            product.Review.Clear();

            if (IsModelValid(product, out error))
            {
                var tem = _Unitcontext.Product.GetById(product.ID);
                product.CreateDateTime = tem.CreateDateTime;
                product.ProductLanguage.Clear();
                //product.UpdateDatetime = DateTime.Now;
                product.BrandId = product.BrandId == 0 ? null : product.BrandId;
                product.UnitId = product.UnitId == 0 ? null : product.UnitId;
                product.UserId = null;
                product.IsAdvertising = true;
                product.Active = false;
                product.IsPublish = null;
                product.MinPrice = product.Price != null ? product.Price.Value : 0;
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
            }
            else
            {
                resultApi.Code = ApiResult.ResponseCode.Error;
                resultApi.Message = error.Body;
            }
            
            //result.Value = obj.ToJson();
            return resultApi;

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
            else if (product.ProductTypeId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTTYPE);
            else if (product.ProductCategoryId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTCATEGORY);
            else if (product.ProductSubCategoryId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTSUBCATEGORY);
            else if (product.BrandId == 0)
                msg = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_PRODUCT_PRODUCTBRAND);
            else if (product.Phone.Length > 11 || product.Phone.Length < 11)
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
