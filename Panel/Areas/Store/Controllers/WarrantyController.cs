using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using DataLayer.ViewModels;
using LinqToExcel;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Xml;

namespace Panel.Areas.Store.Controllers
{
    public class WarrantyController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();

        public ActionResult Index(
            int? index = null,
            int? pagesize = null
           )
        {


            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty);
            int size = pagesize == null ? 50 : pagesize.Value;
            int pageIndex = index == null ? 1 : index.Value;
            ViewBag.PageIndex = index.ToString();
            ViewBag.PageSize = size.ToString();
            ViewBag.TotalCount = _context.Warranty.SearchCount();

            List<Warranty> list = _context.Warranty.Search(
                    pageSize: size,
                    index: pageIndex
                  )
                .ToList();
            return View(list);
        }

        [HttpPost]
        public ActionResult UploadExcel(HttpPostedFileBase file)
        {
            DataSet ds = new DataSet();
            if (Request.Files["file"].ContentLength > 0)
            {
                string fileExtension =
                                     System.IO.Path.GetExtension(Request.Files["file"].FileName);

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["file"].FileName;
                    //if (System.IO.File.Exists(fileLocation))
                    //{

                    //    System.IO.File.Delete(fileLocation);
                    //}
                    Request.Files["file"].SaveAs(fileLocation);
                    string excelConnectionString = string.Empty;
                    excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    //connection String for xls file format.
                    if (fileExtension == ".xls")
                    {
                        excelConnectionString = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 8.0;HDR=Yes;IMEX=2\"";
                    }
                    //connection String for xlsx file format.
                    else if (fileExtension == ".xlsx")
                    {

                        excelConnectionString = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + fileLocation + ";Extended Properties=\"Excel 12.0;HDR=Yes;IMEX=2\"";
                    }
                    //Create Connection to Excel work book and add oledb namespace
                    OleDbConnection excelConnection = new OleDbConnection(excelConnectionString);
                    excelConnection.Open();
                    DataTable dt = new DataTable();

                    dt = excelConnection.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
                    if (dt == null)
                    {
                        return null;
                    }

                    String[] excelSheets = new String[dt.Rows.Count];
                    int t = 0;
                    //excel data saves in temp file here.
                    foreach (DataRow row in dt.Rows)
                    {
                        excelSheets[t] = row["TABLE_NAME"].ToString();
                        t++;
                    }
                    OleDbConnection excelConnection1 = new OleDbConnection(excelConnectionString);


                    string query = string.Format("Select * from [{0}]", excelSheets[0]);
                    using (OleDbDataAdapter dataAdapter = new OleDbDataAdapter(query, excelConnection1))
                    {
                        dataAdapter.Fill(ds);
                    }
                }
                if (fileExtension.ToString().ToLower().Equals(".xml"))
                {
                    string fileLocation = Server.MapPath("~/Content/") + Request.Files["FileUpload"].FileName;
                    if (System.IO.File.Exists(fileLocation))
                    {
                        System.IO.File.Delete(fileLocation);
                    }

                    Request.Files["FileUpload"].SaveAs(fileLocation);
                    XmlTextReader xmlreader = new XmlTextReader(fileLocation);
                    // DataSet ds = new DataSet();
                    ds.ReadXml(xmlreader);
                    xmlreader.Close();
                }

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    var serial = ds.Tables[0].Rows[i][0].ToString();
                    var diditNumber = serial.ToEnglishDigit();
                    var date = ds.Tables[0].Rows[i][2].ToString();
                    var config=ds.Tables[0].Rows[i][4].ToString() ;
                    var name=ds.Tables[0].Rows[i][8].ToString() ;
                    var warranty = _context.Warranty.Where(s => s.Serial == serial).FirstOrDefault();
                    if (warranty != null)
                    {
                        warranty.Date ="14"+ date;
                        warranty.Name = name;
                        warranty.Config = config;
                        _context.Warranty.Update(warranty);
                        _context.Save();
                    }
                    else
                    {
                        Warranty newItem = new Warranty();
                        newItem.Config = config;
                        newItem.Name = name;
                        newItem.Date = "14" + date;
                        newItem.Serial = diditNumber;
                        _context.Warranty.Insert(newItem);
                        _context.Save();
                    }
                }

            }
            return RedirectToAction("Index");
        }
       
       
       
        public ActionResult Create(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty_New);
            ViewBag.ProductType = _context.ProductType.GetById(id);
            return View();
        }

        [HttpPost]
        public ActionResult Create(Warranty warranty)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty_New);
            ViewMessage result = IsModelValid(warranty);
            if (result.Type == Enum_MessageType.SUCCESS)
            {
                warranty.Serial = warranty.Serial.ToEnglishDigit();
                var warrantyTemp = _context.Warranty.Where(s => s.Serial == warranty.Serial).FirstOrDefault();
                if (warrantyTemp != null)
                {
                    result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WARRANTY_SERIAL);
                }
                else
                {
                    _context.Warranty.Insert(warranty);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        public ActionResult Edit(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty_Edit);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Warranty warranty = _context.Warranty.GetById(id);
            if (warranty == null)
                return HttpNotFound();

            ViewBag.Message = BaseMessage.GetMessage();
            return View(warranty);
        }

        [HttpPost]
        public ActionResult Edit(Warranty warranty)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty_Edit);
          
            ViewMessage result = IsModelValid(warranty);
            if (IsModelValid(warranty).Type == Enum_MessageType.SUCCESS)
            {
                warranty.Serial = warranty.Serial.ToEnglishDigit();
                var warrantyTemp = _context.Warranty.Where(s => s.Serial == warranty.Serial && s.ID!=warranty.ID).FirstOrDefault();
                if (warrantyTemp != null)
                {
                    result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WARRANTY_SERIAL);
                }
                else
                {
                    _context.Warranty.Update(warranty);
                    _context.Save();
                }
            }
            return new JsonResult() { Data = result };
        }

        private ViewMessage IsModelValid(Warranty warranty)
        {
            ViewMessage result = new ViewMessage();
            if (warranty.Name == null)
                result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WARRANTY_NAME);
            //else if (warranty.Serial == null)
            //    result = BaseMessage.GetMessage(Enum_MessageType.ERROR, Enum_Message.REQUIRED_WARRANTY_SERIAL);
            return result;
        }

        public ActionResult Delete(int? id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty_Delete);
            if (id == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            Warranty warranty = _context.Warranty.GetById(id);
            if (warranty == null)
                return HttpNotFound();

            return View(warranty);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            _context.Permission.CheckPagePermission(_context.SiteUser.GetCurrentUser(), Enum_Permission.Warranty_Delete);
            Warranty warranty = _context.Warranty.GetById(id);
            try
            {
              
                _context.Warranty.Delete(warranty);
                _context.Save();
                return RedirectToAction("index");
            }
            catch
            {
                ViewBag.Alert = Resource.Notify.CanNotDelete;
                return View(warranty);
            }

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