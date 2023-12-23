using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using DataLayer.Entities;
using System.IO;

namespace CDN.Controllers
{
    public partial class FileController : Controller
    {
        public ActionResult Upload(bool isPrivate = false)
        {
            Response.Headers.Add("Access-Control-Allow-Origin", "*");
            UnitOfWork _context = new UnitOfWork();
            var storagePath = Server.MapPath("~/App_Data/");
            var justNow = DateTime.Now;


            try
            {
                for (int i = 0; i < Request.Files.Count; i++)
                {
                    if (Request.Files[i] != null || Request.Files[i].ContentLength > 0)
                    {
                        var newFile = new DataLayer.Entities.File()
                        {
                            Extension = System.IO.Path.GetExtension(Request.Files[i].FileName),
                            ContentType = Request.Files[i].ContentType,
                            Size = Request.Files[i].ContentLength,
                            Views = 0,
                            Private = isPrivate,
                            CreateDate = justNow,
                            RemoveDate = null,
                        };

                        _context.File.Insert(newFile);
                        _context.Save();

                        try
                        {
                            System.IO.Directory.CreateDirectory(storagePath + @"\File\" + justNow.Year + @"\" + justNow.Month + @"\" + justNow.Day + @"\" + newFile.FileId);
                            string targetPath = storagePath + @"\File\" + justNow.Year + @"\" + justNow.Month + @"\" + justNow.Day + @"\" + newFile.FileId + @"\source.orig";
                            Request.Files[i].SaveAs(targetPath);

                            #region Getting File Properties

                            try
                            {
                                if (newFile.ContentType.IndexOf("image/") == 0)
                                {
                                    if (newFile.ContentType.IndexOf("image/webp") != 0)
                                    {
                                        Image image = Image.FromFile(targetPath);
                                        newFile.Width = image.Width;
                                        newFile.Height = image.Height;
                                    }
                                }
                                else if (newFile.ContentType.IndexOf("video/") == 0)
                                {
                                    if (newFile.ContentType.IndexOf("video/mp4") == 0)
                                    {
                                        TagLib.Mpeg4.File video = new TagLib.Mpeg4.File(targetPath);
                                        newFile.Width = video.Properties.VideoWidth;
                                        newFile.Height = video.Properties.VideoHeight;

                                        video.Dispose();

                                    }
                                }

                                _context.File.Update(newFile);
                                _context.Save();
                                return Json(new { status = "success", FileId = newFile.FileId,Type=newFile.ContentType});
                            }
                            catch { }

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            _context.File.Delete(newFile);
                            _context.Save();

                            return Content(ex.Message);
                        }
                    }
                }

                return Json(new { status = "success" });

            }
            catch (Exception ex)
            {
                return Json(new { status = "failure", message = ex.Message });
            }
        }
    }
}