using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CiellosAzureDashboard.Pages.Applications
{
    [Authorize(Policy = "IsSuperUser")]
    public class IndexModel : PageModel
    {
        private readonly CADContext _context = new CADContext();
        public IndexModel()
        {
        }
        public void OnGet()
        {
            Applications =  _context.Applications.ToList();
        }
        public IList<Application> Applications { get; set; }

    }
}