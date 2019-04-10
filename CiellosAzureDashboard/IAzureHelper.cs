using System.Collections.Generic;
using System.Security.Claims;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Compute.Fluent;
using Newtonsoft.Json.Linq;

namespace CiellosAzureDashboard
{
    public interface IAzureHelper
    {
        IEnumerable<IVirtualMachine> GetAllVirtualMashines();
        Dashboard GetDashboardByAccessCode(string _accessCode);
        JsonResult GetJOVirtualMachine(JObject jobject);
        IVirtualMachine GetVirtualMachine(string _name, string _resourceGroupName, bool _forupdate = false);
        VMList GetVirtualMachinesByAccessCode(string _accessCode);
        JsonResult GetVirtualMachinesByUser(ClaimsPrincipal _currentUser);
    }
}