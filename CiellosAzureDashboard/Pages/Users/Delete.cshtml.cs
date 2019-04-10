using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace CiellosAzureDashboard.Pages.Users
{
    public class DeleteModel : PageModel
    {
        private readonly CADContext _context = new CADContext();

        public DeleteModel()
        {

        }

        [BindProperty]
        public new User User { get; set; }

        public IActionResult OnGet(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = _context.Users.FirstOrDefault(u=>u.UserId == id);

            if (User == null)
            {
                return NotFound();
            }
            return Page();
        }

        public IActionResult OnPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            User = _context.Users.FirstOrDefault(u => u.UserId == id);

            if (User != null)
            {
                _context.Users.Remove(User);
                _context.SaveChanges();
            }

            return RedirectToPage("./Index");
        }
    }
}