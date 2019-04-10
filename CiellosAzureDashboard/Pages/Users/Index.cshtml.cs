using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CiellosAzureDashboard.Pages.Users
{
    [Authorize(Policy = "IsSuperUser")]
    public class IndexModel : PageModel
    {
        public readonly CADContext _context = new CADContext();
        public IndexModel()
        {
        }
        public void OnGetAsync()
        {
            Users =  _context.Users.Include(u=>u.Dashboard).ToList();
        }
        public IList<User> Users { get; set; }

    }
}