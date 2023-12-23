﻿using DataLayer.Base;
using DataLayer.Entities;
using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Threading;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.SessionState;

namespace Panel
{
    public class MvcApplication : System.Web.HttpApplication
    {
        UnitOfWork _context = new UnitOfWork();

        protected void Application_Start()
        {
            ViewEngines.Engines.Clear();
            ViewEngines.Engines.Add(new RazorViewEngine());

            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);

            ThreadPool.QueueUserWorkItem(o => PingServers());
        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {
            if (HttpContext.Current.Request.Url.ToString().ToLower().Contains("http://www."))
            {
                string url = Request.Url.ToString().ToLower();
                url = url.Replace("http://www.", "http://");
                HttpContext.Current.Response.Status = "301 Moved Permanently";
                HttpContext.Current.Response.AddHeader("Location", url);
            }
        }

        protected void Application_PostAuthorizeRequest()
        {
            HttpContext.Current.SetSessionStateBehavior(SessionStateBehavior.Required);
        }

        protected void Application_AcquireRequestState()
        {
            SetWebsiteLanguage();
        }

        private void SetWebsiteLanguage()
        {
            //string currentLanguage = null;
            //if (HttpContext.Current.Session["LANGUAGE"] == null)
            //{
            //    WebsiteDomain domain = _context.WebsiteDomain.GetByUrl(HttpContext.Current.Request.Url);
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
            //else
            //{
            //    currentLanguage = Session["LANGUAGE"].ToString();
            //}

            var currentLang = DataLayer.Base.BaseLanguage.GetCurrentLanguageString();
            Thread.CurrentThread.CurrentUICulture = new CultureInfo(currentLang);
            Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(currentLang);
        }

        private static void PingServers()
        {
            UnitOfWork _context = new UnitOfWork();
            string local_type = Enum_Code.SYSTEM_TYPE_LOCAL.ToString();
            string sync_type = Enum_Code.SYSTEM_TYPE_SYNCSERVER.ToString();
            string api_type = Enum_Code.SYSTEM_TYPE_API.ToString();
            string oauth_type = Enum_Code.SYSTEM_TYPE_OAUTH.ToString();

            while (true)
            {
                List<WebsiteDomain> listDomain = _context.WebsiteDomain.Where(p =>
                    p.Website.Code.Label == local_type ||
                    p.Website.Code.Label == sync_type ||
                    p.Website.Code.Label == api_type ||
                    p.Website.Code.Label == oauth_type).ToList();
                try
                {
                    foreach (WebsiteDomain domain in listDomain)
                    {
                        var webClient = new WebClient();
                        webClient.DownloadString(domain.Domain);
                        Thread.Sleep(TimeSpan.FromMinutes(1));
                    }
                }
                catch (Exception) { }
            }
        }
    }
}
