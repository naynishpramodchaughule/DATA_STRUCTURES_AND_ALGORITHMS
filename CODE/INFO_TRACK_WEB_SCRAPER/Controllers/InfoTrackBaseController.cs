using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace INFO_TRACK_WEB_SCRAPER.Controllers
{   
    public class InfoTrackBaseController : Controller
    {
        // GET: InfoTrackBase
        public virtual ActionResult Index()
        {
            return View();
        }

        /// <summary>
        /// Called when an unhandled exception occurs in the action.
        /// </summary>
        /// <param name="objectExceptionContext">Information about the current request and action.</param>
        protected override void OnException(ExceptionContext objectExceptionContext)
        {
            //Record exception in a persistent system (MongoDB).            

            //Redirect to custom error page.
            objectExceptionContext.ExceptionHandled = true;
            objectExceptionContext.Result = new ViewResult
            {
                ViewName = "~/Views/Shared/Error.cshtml"
            };
        }
    }
}