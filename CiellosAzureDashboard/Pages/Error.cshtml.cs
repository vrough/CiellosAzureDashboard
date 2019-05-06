using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CiellosAzureDashboard.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public class ErrorModel : PageModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet(int statusCode)
        {
            var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

            switch (statusCode)
            {
                case 404:
                    //ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
                    //ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
                case 500:
                    //ViewBag.ErrorMessage = "Sorry something went wrong on the server";
                    //ViewBag.RouteOfException = statusCodeData.OriginalPath;
                    break;
            }
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
        
        //public IActionResult HandleErrorCode(int statusCode)
        //{
        //    var statusCodeData = HttpContext.Features.Get<IStatusCodeReExecuteFeature>();

        //    switch (statusCode)
        //    {
        //        case 404:
        //            //ViewBag.ErrorMessage = "Sorry the page you requested could not be found";
        //            //ViewBag.RouteOfException = statusCodeData.OriginalPath;
        //            break;
        //        case 500:
        //            //ViewBag.ErrorMessage = "Sorry something went wrong on the server";
        //            //ViewBag.RouteOfException = statusCodeData.OriginalPath;
        //            break;
        //    }

        //    return Page();
        //}
    }
}
