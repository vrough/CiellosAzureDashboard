using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace CiellosAzureDashboard.Pages.Settings
{
    [Authorize(Policy = "IsSuperUser")]
    public class IndexModel : PageModel
    {
        public string Message { get; set; }
        public string SaveResult { get; set; }
        private AzureHelper azureHelper;
        private readonly CADContext context = new CADContext();
        [BindProperty]
        public Setting settings { get; set; }
        public string CertExpirationDate { get; set; }
        private IHostingEnvironment _hostingEnvironment;
        public IndexModel(IAzureHelper _azureHelper, CADContext _context, IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
            azureHelper = _azureHelper as AzureHelper;
            context = _context;
            if (context.Settings.FirstOrDefault() == null)
            {
                Setting set = new Setting();
                context.Settings.Add(set);
                context.SaveChanges();
            }
            Message = "Dashboard settings";
        }
        public async Task<IActionResult> OnGetAsync()

        {
            CertExpirationDate = azureHelper.X509Helper.GetRootCAExpirationDate().ToLongDateString();
            settings = await context.Settings.AsNoTracking().FirstOrDefaultAsync();
            if (settings == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var sett = await context.Settings.FindAsync(settings.settingId);
            if (await TryUpdateModelAsync<Setting>(
                 sett,
                 "settings",   
                   c=> c.MaxNumEventsLogStorePerVM))
            {
                await context.SaveChangesAsync();
            }
            SaveResult = "Settings saved.";
            return Page();
        }
        public async Task<IActionResult> OnPostDownload(string filename)
        {
            var cert = azureHelper.GetCertificate();
            var fileArray = cert.GetRawCertData();
            return await Task.Run(()=> File(fileArray, "application/pkix-cert", cert.IssuerName.Name.Replace("CN=", "") + ".cer"));
        }
        public ActionResult OnPostRotateCertificate()
        {
                string folderName = "Upload";
                string webRootPath = _hostingEnvironment.WebRootPath;
                string newPath = Path.Combine(webRootPath, folderName);
                //if (!Directory.Exists(newPath))
                //{
                //    Directory.CreateDirectory(newPath);
                //}
                //foreach (IFormFile item in files)
                //{
                //    if (item.Length > 0)
                //    {
                //        string fileName = ContentDispositionHeaderValue.Parse(item.ContentDisposition).FileName.Trim('"');
                //        string fullPath = Path.Combine(newPath, fileName);
                //        using (var stream = new FileStream(fullPath, FileMode.Create))
                //        {
                //            item.CopyTo(stream);
                //        }
                //    }
                //}
                return this.Content("Success");
            return this.Content("Success");
        }

    }
}