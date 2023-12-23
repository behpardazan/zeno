using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;

namespace Cms
{
    public class MvcApplication : System.Web.HttpApplication
    {
        UnitOfWork _context = new UnitOfWork();



        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ThreadPool.QueueUserWorkItem(o => PingServers());
            
        }

        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        protected void Application_AcquireRequestState()
        {
            IncreaseWebsiteView();
            RemoveWwwUrl();
            string currentUrl = Request.Url.ToString().ToLower();
            

            SetWebsiteLanguage();
        }

        private void PingServers()
        {
            string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
            string sync_type = Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString();
            string api_type = Enum_Code.SYSTEM_TYPE_API.ToString();
            string oauth_type = Enum_Code.SYSTEM_TYPE_OAUTH.ToString();

            List<WebsiteDomain> listDomain = _context.WebsiteDomain.Where(p =>
                    p.Website.Code.Label == local_type ||
                    p.Website.Code.Label == sync_type ||
                    p.Website.Code.Label == api_type ||
                    p.Website.Code.Label == oauth_type).ToList();

            while (true)
            {
                foreach (WebsiteDomain domain in listDomain)
                {
                    string url = domain.Domain.Replace("/api/", "");
                    try
                    {
                        var webClient = new WebClient();
                        webClient.DownloadString(url);
                    }
                    catch (Exception) { }
                }
                Thread.Sleep(TimeSpan.FromMinutes(3));
            }
        }

        private void SetWebsiteLanguage()
        {
            //if (HttpContext.Current.Session["LANGUAGE"] == null)
            //{
            //    WebsiteDomain domain = _context.WebsiteDomain.GetByUrl(Request.Url);
            //    if (domain == null)
            //        Session["LANGUAGE"] = "fa";
            //    else
            //    {
            //        Session["LANGUAGE"] = (domain.LanguageId != null) ? domain.Language.Culture : "fa";
            //        if (string.IsNullOrEmpty(domain.RedirectUrl))
            //        {
            //            Template template = domain.Website.Template;
            //            BaseWebsite.WebsiteLabel = (template != null) ? template.Label : "Default";
            //        }
            //        else
            //        {
            //            Session["LANGUAGE"] = "fa";
            //            string currentUrl = Request.Url.ToString().ToLower();
            //            currentUrl = domain.RedirectUrl;
            //            HttpContext.Current.Response.Redirect(currentUrl);
            //        }
            //    }
            //}
            //Thread.CurrentThread.CurrentUICulture = new CultureInfo(Session["LANGUAGE"].ToString());
            //Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(Session["LANGUAGE"].ToString());
            var currentLang = DataLayer.Base.BaseLanguage.GetCurrentLanguageString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
        }

        private void OnlineTajerRedirect()
        {
            string currentUrl = Request.Url.ToString().ToLower();
            if (currentUrl.Contains("onlinetajer.net") ||
                currentUrl.Contains("onlinetajer.org"))
            {
                string url = Request.Url.ToString().ToLower();
                url = url.Replace(".net", ".com");
                url = url.Replace(".org", ".com");
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
            }
        }
        private void RemoveWwwUrl()
        {
            string currentUrl = Request.Url.ToString().ToLower();
            if (currentUrl.Contains("http://www.") ||
                currentUrl.Contains("https://www."))
            {
                string url = Request.Url.ToString().ToLower();
                url = url.Replace("https://www.", "https://");
                url = url.Replace("http://www.", "http://");
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
            }
        }



        private void ChangeUrl()
        {
            string currentUrl = Request.Url.ToString().ToLower();
            if (currentUrl.Contains("-pr-"))
            {
                string url = Request.Url.ToString().ToLower();
                string[] test = url.Split('/');
                string id = test[4];
                url = url.Replace("-pr-"+ id+"-", "");
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
            }
        }
        protected void Session_Start(object sender, EventArgs e)
        {
            Session["ShowMessage"] = "1";
        }
        private void IncreaseWebsiteView()
        {
            try
            {
                if (Session["SITEVIEW"] == null)
                {
                    Session["SITEVIEW"] = Session.SessionID;
                    try
                    {
                        _context.WebsiteView.IncreaseView();
                        _context.Save();
                    }
                    catch (Exception ex)
                    {
                        string msg = ex.Message;
                    }
                }
            }
            catch
            {

            }
        }


        protected void Application_BeginRequest()
        {        
            string url = Request.Url.ToString()/*.Replace(" ","")*/.Trim();

           
        }


       

    }

}
