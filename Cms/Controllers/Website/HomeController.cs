using System;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;

using DataLayer.Base;
using DataLayer.Entities;



namespace Cms.Controllers
{
    public class HomeController : Controller
    {
        private UnitOfWork _context = new UnitOfWork();
        //[OutputCache(Duration = 300, VaryByParam = "none")]
        public ActionResult Index()
        {

            string fullUrl = Request.Url.Host;
            string wildcardDomain = BaseWebsite.WildCardDomain;
            if (wildcardDomain != null && wildcardDomain != "")
            {
                string[] domains = fullUrl.Split(new char[] { '.' });
                if (domains.Length > 1)
                {
                    string subDomain = domains[0];
                    Uri newUrl = new Uri(wildcardDomain.Replace("*", subDomain));
                    if (newUrl.Host == fullUrl)
                    {
                        ProductBrand brand = _context.ProductBrand.GetByLabel(subDomain);
                        if (brand != null)
                        {
                            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/ProductBrand/Index.cshtml", brand);
                        }
                    }
                }
            }
            ViewBag.PageName = "HomePage";
            return PartialView(BaseController.GetView(this));
        }
        [OutputCache(Duration = 1800)]

        public ActionResult Footer()
        {
            return PartialView("~/Views/" + BaseWebsite.WebsiteLabel + "/Shared/_Footer.cshtml");

        }
        public ActionResult Test()
        {
            UnitOfWork _unit = new UnitOfWork();


            return PartialView(BaseController.GetView(this));
        }
        public ActionResult SendEmail()
        {
            using (MailMessage mm = new MailMessage("info@drshamsgroup.com", "mortezazand3@gmail.com"))
            {
                mm.Subject = "hi";
                mm.Body = "hello world";

                mm.IsBodyHtml = false;
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mail.drshamsgroup.com";
                smtp.EnableSsl = true;
                NetworkCredential NetworkCred = new NetworkCredential("info@drshamsgroup.com", "znsv*kbco2r&4mxpjltiqdOyaCufwe");
                smtp.UseDefaultCredentials = true;
                smtp.Credentials = NetworkCred;
                smtp.Port = 587;
                smtp.Send(mm);
                return null;
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