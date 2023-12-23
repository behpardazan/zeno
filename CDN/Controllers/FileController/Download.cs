using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Text.RegularExpressions;
using System.Collections;
using System.Drawing;
using System.IO;
using System.Drawing.Imaging;
using DataLayer.Entities;
using DataLayer.Enumarables;
using Lib.Web.Mvc;

namespace CDN.Controllers
{
    public partial class FileController : Controller
    {
        public ActionResult Download(long fileId)
        {
            Response.AddHeader("Last-Processing", DateTime.Now.ToString());

            UnitOfWork _context = new UnitOfWork();
            var storagePath = Server.MapPath("~/App_Data/");

            var returnedFile = _context.File.FirstOrDefault(s => s.FileId == fileId);

            if (returnedFile != null)
            {
                if (returnedFile.Private == true)
                {
                    bool active = false;
                    var account = _context.Account.GetCurrentAccount();
                    if (account == null)
                        return Redirect("/");
                    else
                    {
                        var order = _context.AccountOrder.Where(s => s.AccountId == account.Id).FirstOrDefault();
                        if (order != null)
                        {
                            var product = order.AccountOrderProduct.Where(s => s.AccountOrder.Code.Label == Enum_Code.ORDER_STATUS_SUCCESS.ToString());
                            foreach (var item in product)
                            {
                                if (item.Product.MusicFileId == fileId || item.Product.PdfFileId == fileId || item.Product.VideoFileId == fileId)
                                {
                                    active = true;
                                }
                            }
                            if (active == true)
                            {
                                returnedFile.Views += 1;
                                _context.File.Update(returnedFile);
                                _context.Save();

                                DateTime targetDate = DateTime.Now;
                                targetDate = targetDate.AddDays(targetDate.DayOfWeek - DayOfWeek.Monday).ToUniversalTime();
                                DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);
                                Response.Cache.SetExpires(expireDate);
                                Response.AddHeader("Accept-Ranges", "bytes");
                                Response.AddHeader("content-disposition", "attachment; filename=" + returnedFile.FileId + returnedFile.Extension);
                                Response.ContentType = returnedFile.Extension;
                                //var test = storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                                //         returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                                //         returnedFile.FileId + @"\source.orig";
                                //return new RangeFileContentResult(returnedFile.Content, returnedFile.ContentType, returnedFile.FileId.ToString(), DateTime.Now);
                                return
                                 File(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                                    returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                                    returnedFile.FileId + @"\source.orig", returnedFile.ContentType);
                            }
                            else
                            {
                                return Redirect("/");
                            }
                        }

                        else
                        {
                            return Redirect("/");
                        }
                    }
                }
                else
                {
                    returnedFile.Views += 1;
                    _context.File.Update(returnedFile);
                    _context.Save();

                    DateTime targetDate = DateTime.Now;
                    targetDate = targetDate.AddDays(targetDate.DayOfWeek - DayOfWeek.Monday).ToUniversalTime();
                    DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);
                    Response.Cache.SetExpires(expireDate);

                    Response.AddHeader("content-disposition", "attachment; filename=" + returnedFile.FileId + returnedFile.Extension);
                    var test = storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                                       returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                                       returnedFile.FileId + @"\source.orig";
                    return
                        File(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                             returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                             returnedFile.FileId + @"\source.orig", returnedFile.ContentType);
                }

            }

            throw new FileNotFoundException();
        }
        public ActionResult Download2(long fileId)
        {
            Response.AddHeader("Last-Processing", DateTime.Now.ToString());

            UnitOfWork _context = new UnitOfWork();
            var storagePath = Server.MapPath("~/App_Data/");

            var returnedFile = _context.File.FirstOrDefault(s => s.FileId == fileId);

            if (returnedFile != null)
            {
                if (returnedFile.Private == true)
                {
                    bool active = false;
                    var account = _context.Account.GetCurrentAccount();
                    if (account == null)
                        return Redirect("/");
                    else
                    {
                        //var order = _context.AccountOrder.Where(s => s.AccountId == account.Id).Where(x=>x.ParentId!=null).FirstOrDefault();
                        //if (order != null)
                        //{
                        var product = _context.AccountOrderProduct.Where(s => s.AccountOrder.StatusId == 2068 && s.AccountOrder.AccountId == account.Id).ToList();
                        foreach (var item in product)
                        {
                            if (item.Product.MusicFileId == fileId || item.Product.PdfFileId == fileId || item.Product.VideoFileId == fileId)
                            {
                                active = true;
                            }
                        }
                        if (active == true)
                        {
                            returnedFile.Views += 1;
                            _context.File.Update(returnedFile);
                            _context.Save();

                            DateTime targetDate = DateTime.Now;
                            targetDate = targetDate.AddDays(targetDate.DayOfWeek - DayOfWeek.Monday).ToUniversalTime();
                            DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);
                            Response.Cache.SetExpires(expireDate);
                            Response.AddHeader("Accept-Ranges", "bytes");
                            Response.AddHeader("content-disposition", "attachment; filename=" + returnedFile.FileId + returnedFile.Extension);
                            Response.ContentType = returnedFile.Extension;
                            var filename = storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                                     returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                                     returnedFile.FileId + @"\source.orig";
                            byte[] bytes = System.IO.File.ReadAllBytes(filename);
                            //Save the Byte Array as File.
                            //FileInfo fil = new FileInfo("A://1/test.mp4");

                            //using (Stream sw = fil.OpenWrite())
                            //{
                            //    sw.Write(bytes, 0, bytes.Length);
                            //    sw.Close();
                            //}

                            return new RangeFileContentResult(bytes, returnedFile.ContentType, returnedFile.FileId.ToString(), DateTime.Now);
                        }
                        else
                        {
                            return Redirect("/");
                        }
                        //}

                        //else
                        //{
                        //    return Redirect("/");
                        //}
                    }
                }
                else
                {
                    returnedFile.Views += 1;
                    _context.File.Update(returnedFile);
                    _context.Save();

                    DateTime targetDate = DateTime.Now;
                    targetDate = targetDate.AddDays(targetDate.DayOfWeek - DayOfWeek.Monday).ToUniversalTime();
                    DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);

                    Response.Cache.SetExpires(expireDate);
                    Response.AddHeader("Accept-Ranges", "bytes");
                    Response.AddHeader("content-disposition", "attachment; filename=" + returnedFile.FileId + returnedFile.Extension);
                    Response.ContentType = returnedFile.Extension;
                    var filename = storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                             returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                             returnedFile.FileId + @"\source.orig";
                    byte[] bytes = System.IO.File.ReadAllBytes(filename);
                    //FileInfo fil = new FileInfo("A://1/test.mp4");

                    //using (Stream sw = fil.OpenWrite())
                    //{
                    //    sw.Write(bytes, 0, bytes.Length);
                    //    sw.Close();
                    //}
                    return new RangeFileContentResult(bytes, returnedFile.ContentType, returnedFile.FileId.ToString(), DateTime.Now);

                    //Response.AddHeader("content-disposition", "attachment; filename=" + returnedFile.FileId + returnedFile.Extension);
                    //var test = storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                    //                   returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                    //                   returnedFile.FileId + @"\source.orig";
                    //return
                    //    File(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                    //         returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                    //         returnedFile.FileId + @"\source.orig", returnedFile.ContentType);
                }

            }

            throw new FileNotFoundException();
        }

    }
}