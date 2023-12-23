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
using CDN.Helpers;
using DataLayer.Entities;
using DataLayer.Enumarables;
using Lib.Web.Mvc;
namespace CDN.Controllers
{
    public partial class FileController : Controller
    {
        public ActionResult Thumb(long fileId, int width, int height, long quality)
        {
            UnitOfWork _context = new UnitOfWork();
            var storagePath = Server.MapPath("~/App_Data/");
            var returnedFile = _context.File.FirstOrDefault(s => s.FileId == fileId);

            returnedFile.Views += 1;
            _context.File.Update(returnedFile);
            _context.Save();

            #region Thumb From Cache
            if (HttpRuntime.Cache[string.Format("CDNFileThumb({0}x{1}x{2}x{3})", fileId, width, height, quality)] != null)
            {
                try
                {
                    Stream resultStream = (new MemoryStream());
                    Stream cacheStream = ((MemoryStream)HttpRuntime.Cache[string.Format("CDNFileThumb({0}x{1}x{2}x{3})", fileId, width, height, quality)]);
                    cacheStream.Position = 0;
                    cacheStream.CopyTo(resultStream);

                    resultStream.Position = 0;
                    Response.AddHeader("x-from-cache", "true");
                    return File(resultStream, "image/jpeg");
                }
                catch { }
            }
            #endregion

            if (returnedFile != null)
            {
                DateTime targetDate = DateTime.Now;
                targetDate = targetDate.AddMonths(1).ToUniversalTime();
                DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);
                Response.Cache.SetExpires(expireDate);

                if (returnedFile.ContentType.ToLower().IndexOf("image/") == 0)
                {
                    #region Processing Image Files
                    Image sourceFile = new Bitmap(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" + returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" + returnedFile.FileId + @"\source.orig");
                    Image resultImage = new Bitmap(width, height);

                    Stream stream = (new MemoryStream());

                    if (sourceFile.Height * width / sourceFile.Width > height)
                    {
                        sourceFile = new Bitmap(sourceFile, width, sourceFile.Height * width / sourceFile.Width);
                    }
                    else
                    {
                        sourceFile = new Bitmap(sourceFile, sourceFile.Width * height / sourceFile.Height, height);
                    }

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                        graphics.FillRectangle(Brushes.White, 0, 0, resultImage.Width, resultImage.Height);

                        graphics.DrawImage(sourceFile, new Rectangle(0, 0, width, height), new Rectangle(new Point((sourceFile.Width - width) / 2, height > width ? (sourceFile.Height - height) / 2 : 0), new System.Drawing.Size(width, height)), GraphicsUnit.Pixel);

                        EncoderParameters parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                        resultImage.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault(s => s.MimeType == "image/jpeg"), parameters);
                        stream.Position = 0;

                        #region Caching this Thumb
                        Stream cacheStram = (new MemoryStream());
                        stream.CopyTo(cacheStram);
                        HttpContext.Cache.GetOrStore(string.Format("CDNFileThumb({0}x{1}x{2}x{3})", fileId, width, height, quality), cacheStram);
                        stream.Position = 0;
                        #endregion

                        return File(stream, returnedFile.ContentType);
                    }
                    #endregion
                }
                else
                {
                    #region Processing other files
                    Image icon = null;
                    Image resultImage = new Bitmap(width, height, PixelFormat.Format24bppRgb);

                    if (returnedFile.ContentType.ToLower().IndexOf("audio/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Audio.png"));
                    }
                    else if (returnedFile.ContentType.ToLower().IndexOf("video/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Video.png"));
                    }
                    else if (returnedFile.ContentType.ToLower().IndexOf("text/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Text.png"));
                    }
                    else if (returnedFile.ContentType.ToLower().IndexOf("application/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Application.png"));
                    }
                    else
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Unknow.png"));
                    }

                    if (resultImage.Height < icon.Height)
                    {
                        int targetHeight = resultImage.Height;
                        icon = new Bitmap(icon, new System.Drawing.Size(icon.Width * targetHeight / icon.Height, targetHeight));
                    }

                    Stream stream = (new MemoryStream());

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(11, 11, 11)), Rectangle.FromLTRB(0, 0, resultImage.Width, resultImage.Height));
                        graphics.DrawImage(icon, new Rectangle(resultImage.Width / 2 - icon.Width / 2, resultImage.Height / 2 - icon.Height / 2, icon.Width, icon.Height));
                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(40, 55, 55, 55)), Rectangle.FromLTRB(0, 0, resultImage.Width, resultImage.Height));

                        EncoderParameters parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);

                        resultImage.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault(s => s.MimeType == "image/jpeg"), parameters);
                        stream.Position = 0;

                        #region Caching this Thumb
                        Stream cacheStram = (new MemoryStream());
                        stream.CopyTo(cacheStram);
                        HttpContext.Cache.GetOrStore(string.Format("CDNFileThumb({0}x{1}x{2}x{3})", fileId, width, height, quality), cacheStram);
                        stream.Position = 0;
                        #endregion

                        return File(stream, "image/jpeg");
                    }
                    #endregion
                }
            }

            throw new Exception();
        }

        public ActionResult ThumbSquare(long fileId, int size, long quality)
        {
            UnitOfWork dbContext = new UnitOfWork();
            var storagePath = Server.MapPath("~/App_Data/");
            var returnedFile = dbContext.File.FirstOrDefault(s => s.FileId == fileId);

            returnedFile.Views += 1;
            dbContext.File.Update(returnedFile);
            dbContext.Save();

            #region Thumb From Cache
            if (HttpRuntime.Cache[string.Format("CDNFileThumbSquare({0}x{1}x{2})", fileId, size, quality)] != null)
            {
                try
                {
                    Stream resultStream = (new MemoryStream());
                    Stream cacheStream = ((MemoryStream)HttpRuntime.Cache[string.Format("CDNFileThumbSquare({0}x{1}x{2})", fileId, size, quality)]);
                    cacheStream.Position = 0;
                    cacheStream.CopyTo(resultStream);

                    resultStream.Position = 0;
                    Response.AddHeader("x-from-cache", "true");
                    return File(resultStream, "image/jpeg");
                }
                catch { }
            }
            #endregion

            if (returnedFile != null)
            {
                DateTime targetDate = DateTime.Now;
                targetDate = targetDate.AddMonths(1).ToUniversalTime();
                DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);
                Response.Cache.SetExpires(expireDate);

                if (returnedFile.ContentType.ToLower().IndexOf("image/") == 0)
                {
                    #region Processing Image Files
                    Image sourceFile = new Bitmap(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" + returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" + returnedFile.FileId + @"\source.orig");
                    var targetWidth = 0;
                    var targetHeight = 0;
                    if (sourceFile.Width > sourceFile.Height)
                    {
                        targetWidth = sourceFile.Width > size ? size : sourceFile.Width;
                        targetHeight = sourceFile.Height * size / sourceFile.Width;
                    }
                    else
                    {
                        targetHeight = sourceFile.Height > size ? size : sourceFile.Height;
                        targetWidth = sourceFile.Width * size / sourceFile.Height;
                    }
                    sourceFile = new Bitmap(sourceFile, targetWidth, targetHeight);
                    Image resultImage = new Bitmap(size, size);

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                        graphics.FillRectangle(Brushes.White, 0, 0, resultImage.Width, resultImage.Height);

                        graphics.DrawImage(sourceFile
                            , new Rectangle((resultImage.Width - targetWidth) / 2, (resultImage.Height - targetHeight) / 2, targetWidth, targetHeight)
                            , new Rectangle(0, 0, sourceFile.Width, sourceFile.Height), GraphicsUnit.Pixel);
                    }

                    Stream stream = (new MemoryStream());
                    EncoderParameters parameters = new EncoderParameters(1);
                    parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                    resultImage.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault(s => s.MimeType == "image/jpeg"), parameters);
                    stream.Position = 0;

                    #region Caching this Thumb
                    HttpContext.Cache.GetOrStore(string.Format("CDNFileThumbSquare({0}x{1}x{2})", fileId, size, quality), stream);
                    stream.Position = 0;
                    #endregion

                    return File(stream, returnedFile.ContentType);

                    #endregion
                }
                else
                {
                    #region Processing other files
                    Image icon = null;
                    Image resultImage = new Bitmap(size, size, PixelFormat.Format24bppRgb);

                    if (returnedFile.ContentType.ToLower().IndexOf("audio/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Audio.png"));
                    }
                    else if (returnedFile.ContentType.ToLower().IndexOf("video/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Video.png"));
                    }
                    else if (returnedFile.ContentType.ToLower().IndexOf("text/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Text.png"));
                    }
                    else if (returnedFile.ContentType.ToLower().IndexOf("application/") == 0)
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Application.png"));
                    }
                    else
                    {
                        icon = new Bitmap(Server.MapPath("~/Icons/Thumb/Unknow.png"));
                    }

                    if (resultImage.Height < icon.Height)
                    {
                        int targetHeight = resultImage.Height;
                        icon = new Bitmap(icon, new System.Drawing.Size(icon.Width * targetHeight / icon.Height, targetHeight));
                    }

                    Stream stream = (new MemoryStream());

                    using (Graphics graphics = Graphics.FromImage(resultImage))
                    {
                        graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                        graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                        graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                        graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(11, 11, 11)), Rectangle.FromLTRB(0, 0, resultImage.Width, resultImage.Height));
                        graphics.DrawImage(icon, new Rectangle(resultImage.Width / 2 - icon.Width / 2, resultImage.Height / 2 - icon.Height / 2, icon.Width, icon.Height));
                        graphics.FillRectangle(new SolidBrush(System.Drawing.Color.FromArgb(40, 55, 55, 55)), Rectangle.FromLTRB(0, 0, resultImage.Width, resultImage.Height));

                        EncoderParameters parameters = new EncoderParameters(1);
                        parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, 99L);

                        resultImage.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault(s => s.MimeType == "image/jpeg"), parameters);
                        stream.Position = 0;

                        #region Caching this Thumb
                        Stream cacheStram = (new MemoryStream());
                        stream.CopyTo(cacheStram);
                        HttpContext.Cache.GetOrStore(string.Format("CDNFileThumbSquare({0}x{1}x{2})", fileId, size, quality), cacheStram);
                        stream.Position = 0;
                        #endregion

                        return File(stream, "image/jpeg");
                    }
                    #endregion
                }
            }

            throw new FileNotFoundException();
        }

        public ActionResult ThumbWidth(long fileId, int width, long quality)
        {
            UnitOfWork dbContext = new UnitOfWork();
            var storagePath = Server.MapPath("~/App_Data/");
            var returnedFile = dbContext.File.FirstOrDefault(s => s.FileId == fileId);

            returnedFile.Views += 1;
            dbContext.File.Update(returnedFile);
            dbContext.Save();

            #region Thumb From Cache
            if (HttpRuntime.Cache[string.Format("CDNFileThumbWidth({0}x{1}x{2})", fileId, width, quality)] != null)
            {
                try
                {
                    Stream resultStream = (new MemoryStream());
                    Stream cacheStream = ((MemoryStream)HttpRuntime.Cache[string.Format("CDNFileThumbWidth({0}x{1}x{2})", fileId, width, quality)]);
                    cacheStream.Position = 0;
                    cacheStream.CopyTo(resultStream);

                    resultStream.Position = 0;
                    Response.AddHeader("x-from-cache", "true");
                    return File(resultStream, "image/jpeg");
                }
                catch { }
            }
            #endregion

            Response.AddHeader("Last-Processing", DateTime.Now.ToString());

            if (returnedFile != null)
            {
                DateTime targetDate = DateTime.Now;
                targetDate = targetDate.AddMonths(1).ToUniversalTime();
                DateTime expireDate = new DateTime(targetDate.Year, targetDate.Month, targetDate.Day, 1, 1, 0, 0);
                Response.Cache.SetExpires(expireDate);

                Image sourceFile = new Bitmap(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" + returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" + returnedFile.FileId + @"\source.orig");
                if (width == 0)
                    width = sourceFile.Width;
                int targetwidth = sourceFile.Width <= width ? sourceFile.Width : width;

                Image resultImage = new Bitmap(targetwidth, sourceFile.Height * targetwidth / sourceFile.Width);

                using (Graphics graphics = Graphics.FromImage(resultImage))
                {
                    graphics.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceOver;
                    graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.HighSpeed;
                    graphics.CompositingQuality = System.Drawing.Drawing2D.CompositingQuality.HighSpeed;
                    graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.None;

                    graphics.FillRectangle(Brushes.White, 0, 0, resultImage.Width, resultImage.Height);

                    graphics.DrawImage(sourceFile, new Rectangle(0, 0, resultImage.Width, resultImage.Height),
                        new Rectangle(0, 0, sourceFile.Width, sourceFile.Height), GraphicsUnit.Pixel);
                }

                Stream stream = (new MemoryStream());
                EncoderParameters parameters = new EncoderParameters(1);
                parameters.Param[0] = new EncoderParameter(System.Drawing.Imaging.Encoder.Quality, quality);

                resultImage.Save(stream, ImageCodecInfo.GetImageEncoders().FirstOrDefault(s => s.MimeType == "image/jpeg"), parameters);
                stream.Position = 0;

                #region Caching this Thumb
                Stream cacheStram = (new MemoryStream());
                stream.CopyTo(cacheStram);
                HttpContext.Cache.GetOrStore(string.Format("CDNFileThumbWidth({0}x{1}x{2})", fileId, width, quality), cacheStram);
                stream.Position = 0;
                #endregion

                return File(stream, returnedFile.ContentType);
            }

            throw new FileNotFoundException();
        }
        public ActionResult ThumbVideo(long fileId)
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

                                Response.AddHeader("content-disposition", "attachment; filename=" + returnedFile.FileId + returnedFile.Extension);
                                var test = storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                                         returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                                         returnedFile.FileId + @"\source.orig";
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
                    var file = File(storagePath + @"\File\" + returnedFile.CreateDate.Year + @"\" +
                           returnedFile.CreateDate.Month + @"\" + returnedFile.CreateDate.Day + @"\" +
                             returnedFile.FileId + @"\source.orig", returnedFile.ContentType);
                    //RangeFileContentResult
                    byte[] fileBytes = System.IO.File.ReadAllBytes("some_file.mp3");
                    //return File(fileBytes, returnedFile.ContentType, test);
                    return new RangeFileContentResult(fileBytes, returnedFile.ContentType, file.FileName, DateTime.Now);

                }

            }

            throw new FileNotFoundException();
        }
    }
}