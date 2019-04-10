using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;

namespace CiellosAzureDashboard.Pages.Dashboards
{
    [Authorize(Policy = "IsSuperUser")]
    public class IndexModel : PageModel
    {
        public CADContext _context;
        
        public IndexModel()
        {
            _context = new CADContext();
        }

        public void OnGet()
        {
            Dashboards =   _context.Dashboards.Include(d=>d.DashboardApplications).ToList();
        }
        public ICollection<Dashboard> Dashboards { get; set; }

    }
}