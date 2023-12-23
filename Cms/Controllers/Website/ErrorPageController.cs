﻿using DataLayer.Enumarables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Cms.Controllers.Website
{
    public class ErrorPageController : Controller
    {


        private static readonly List<int> StatusCode = new List<int>
        {
            100, 101,
            200, 201, 202, 203, 204, 205, 206,
            300, 301, 302, 303, 304, 305, 306, 307,
            400, 401, 402, 403, 404, 405, 406, 407, 408, 409, 410, 411, 412, 413, 414, 415, 416, 417, 426,
            500, 501, 502, 503, 504, 505
        };

        private static int HttpStatusCode(int id)
        {
            return StatusCode.Contains(id) ? id : 404;
        }

        public ActionResult Index(int id = 404)
        {
            Response.StatusCode = HttpStatusCode(id);
            return View();
        }
    }
}