using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Management.Compute.Fluent;
using Microsoft.Azure.Management.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent;
using Microsoft.Azure.Management.ResourceManager.Fluent.Authentication;
using Microsoft.Azure.Management.ResourceManager.Fluent.Core;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

namespace CiellosAzureDashboard
{
    public class AzureHelper : IAzureHelper
    {
        private readonly CADContext context;
        private List<IVirtualMachine> VirtualMachines { get; set; }
        public X509Helper X509Helper { get; set; }
        public AzureHelper(CADContext _context, IX509Helper _X509Helper)
        {
            X509Helper = _X509Helper as X509Helper;
            context = _context;
            X509Helper.RotateCertificate();
        }
        /// <summary>
        /// LoadCertificate
        /// </summary>
        private void LoadCertificate()
        {
           // X509Helper.RotateCertificate();
           // environmentCertificate = X509Helper.GetPublicKeyCertificate();
            //string thumbprint = Environment.GetEnvironmentVariable("APPSETTING_THUMBPRINT");
            //if (environmentCertificate == null && !string.IsNullOrEmpty(thumbprint))
            //{
            //    environmentCertificate = GetCertificate(thumbprint);
            //}
        }
        /// <summary>
        /// GetAzureConnection
        /// </summary>
        /// <param name="_app">Application instance</param>
        /// <returns></returns>
        private IAzure GetAzureConnection(Application _app)
        {
            if (!String.IsNullOrEmpty(_app.ClientSecret))
            {
                return GetAzureConnection(_app.ClientId,
                               _app.ClientSecret,
                               _app.TenantId,
                               _app.SubscriptionId);
            }
            else
            {
                return GetAzureConnection(_app.ClientId,
                                          _app.TenantId,
                                          _app.SubscriptionId);
            }
        }
        /// <summary>
        /// GetAzureConnection object
        /// </summary>
        /// <param name="_clientId">Application client identifier</param>
        /// <param name="_clientSecret">Application secret key</param>
        /// <param name="_tenantId">Tenant identifier</param>
        /// <param name="_subscriptionId">Subscription inentifier</param>
        /// <returns></returns>
        private IAzure GetAzureConnection(string _clientId, string _clientSecret, string _tenantId, string _subscriptionId)
        {
            var creds = new AzureCredentialsFactory().FromServicePrincipal(_clientId, _clientSecret, _tenantId, AzureEnvironment.AzureGlobalCloud);
            var azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(creds)
                .WithSubscription(_subscriptionId);
            return azure;
        }
        public X509Certificate2 GetCertificate()
        {
            return X509Helper.GetPublicKeyCertificate();
        }
        /// <summary>
        /// GetCertificate method
        /// </summary>
        /// <param name="_thumbPrint">certificate thumbPrint</param>
        /// <returns>X509Certificate2 instance</returns>
        //private X509Certificate2 GetCertificate(string _thumbPrint)
        //{
        //    if (environmentCertificate != null) return environmentCertificate;
        //    if (string.IsNullOrEmpty(_thumbPrint))
        //        throw new ArgumentNullException("thumbprint", "Argument 'thumbprint' cannot be 'null' or 'string.empty'");

        //    X509Store certStore = new X509Store(StoreName.My, StoreLocation.CurrentUser);
        //    X509Certificate2 cert = new X509Certificate2();
        //    try
        //    {
        //        certStore.Open(OpenFlags.ReadOnly);
        //        X509Certificate2Collection certCollection = certStore.Certificates.Find(
        //                                    X509FindType.FindByThumbprint,
        //                                    _thumbPrint,
        //                                    false);

        //        if (certCollection.Count > 0)
        //        {
        //            cert = certCollection[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    finally
        //    {
        //        if (certStore != null)
        //        {
        //            certStore.Close();
        //        }
        //    }
        //    return cert;
        //}

        /// <summary>
        /// GetAzureConnection
        /// </summary>
        /// <param name="_clientId"></param>
        /// <param name="_tenantId"></param>
        /// <returns></returns>
        private IAzure GetAzureConnection(string _clientId, string _tenantId, string _subscriptionId)
        {
            IAzure azure;
            var creds = new AzureCredentialsFactory().FromServicePrincipal(_clientId, X509Helper.GetRootCertificate(), _tenantId, AzureEnvironment.AzureGlobalCloud);
            azure = Azure
                .Configure()
                .WithLogLevel(HttpLoggingDelegatingHandler.Level.Basic)
                .Authenticate(creds)
                .WithSubscription(_subscriptionId);
            return azure;
        }
        /// <summary>
        /// GetAllVirtualMashines
        /// </summary>
        /// <returns>list of all virtual mashines by all applications</returns>
        public IEnumerable<IVirtualMachine> GetAllVirtualMashines()
        {
            VirtualMachines = new List<IVirtualMachine>();
            IAzure azure;
            foreach (var apps in context.Applications)
            {
                try
                {
                    azure = GetAzureConnection(apps);
                    VirtualMachines.AddRange(azure.VirtualMachines.List());
                }
                catch
                {
                    continue;
                }
            }
            return VirtualMachines;
        }

        /// <summary>
        /// GetVirtualMachine
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_resourceGroupName"></param>
        /// <param name="_forupdate"></param>
        /// <returns></returns>
        public IVirtualMachine GetVirtualMachine(string _name, string _resourceGroupName, bool _forupdate = false)
        {
            if (_forupdate)
            {
                GetAllVirtualMashines();
            }
            else
            {
                if (VirtualMachines == null || VirtualMachines?.Count == 0)
                {
                    GetAllVirtualMashines();
                }
            }
            return VirtualMachines.FirstOrDefault(vm => vm.Name == _name && vm.ResourceGroupName == _resourceGroupName);
        }

        /// <summary>
        /// GetJOVirtualMachineAsync
        /// </summary>
        /// <param name="jobject"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetJOVirtualMachineAsync(JObject jobject)
        {
            return await Task.Run(() => GetJOVirtualMachine(jobject));
        }
        /// <summary>
        /// createLocalVMFromAzureVM
        /// </summary>
        /// <param name="_virtualMachine"></param>
        /// <returns></returns>
        private VM createLocalVMFromAzureVM(IVirtualMachine _virtualMachine)
        {
            VM vm = new VM();

            vm.VMName = _virtualMachine.Name;
            vm.ResourceGroupName = _virtualMachine.ResourceGroupName;
            vm.VMSize = _virtualMachine.Size.ToString();

            string vmPowerState = _virtualMachine.PowerState?.ToString();
            if (!String.IsNullOrEmpty(vmPowerState))
            {
                if (vmPowerState.StartsWith("PowerState/"))
                {
                    vmPowerState = vmPowerState.Remove(0, "PowerState/".Length);
                }
                vm.PowerState = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(vmPowerState);
            }
            
            string tagRows = "";
            foreach (var tag in _virtualMachine.Tags)
            {
                tagRows += "<div class='zoom'><span class='TagRow'>";
                tagRows += "<span class='Tag'>" + tag.Key.ToString();
                tagRows += "</span>";
                tagRows += "<span class='Value'>" + tag.Value.ToString();
                tagRows += "</span>";
                tagRows += "</span></div>";
            }

            vm.Tags = tagRows;
            vm.ProvisioningState = _virtualMachine.ProvisioningState;
            vm.VMId = _virtualMachine.VMId;
            vm.SubscriptionId = _virtualMachine.Manager.SubscriptionId;

            return vm;
        }
        /// <summary>
        /// GetJOVirtualMachine
        /// </summary>
        /// <param name="jobject"></param>
        /// <returns></returns>
        public JsonResult GetJOVirtualMachine(JObject jobject)
        {
            VM vm = new VM();
            try
            {
                IVirtualMachine virtualMachine = GetVirtualMachine(jobject["vmname"].ToString(), jobject["resourcegroup"].ToString(), true);
                vm = createLocalVMFromAzureVM(virtualMachine);
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.name = ex.ToString();
                log.timestamp = DateTime.UtcNow;
                context.Logs.Add(log);
                context.SaveChanges();
            }
            return new JsonResult(new { data = vm });
        }
        /// <summary>
        /// GetVirtualMachinesByUserAsync
        /// </summary>
        /// <param name="_currentUser"></param>
        /// <returns></returns>
        public async Task<JsonResult> GetVirtualMachinesByUserAsync(ClaimsPrincipal _currentUser)
        {
            return await Task.Run(() => GetVirtualMachinesByUser(_currentUser));
        }
        /// <summary>
        /// GetVirtualMachinesByUser
        /// </summary>
        /// <param name="_currentUser"></param>
        /// <returns></returns>
        public JsonResult GetVirtualMachinesByUser(ClaimsPrincipal _currentUser)
        {
            List<Application> applications = new List<Application>();
            List<VM> vMashines = new List<VM>();
            try
            {
                User currentUser = context.Users
                    .Include(u => u.Dashboard)
                    .ThenInclude(d => d.DashboardApplications)
                    .FirstOrDefault(u => u.UserName == _currentUser.Identity.Name);
                if (currentUser.Dashboard == null) throw null;

                var dashboard = context.Dashboards.Include(d => d.DashboardApplications).FirstOrDefault(d => d.DashboardId == currentUser.Dashboard.DashboardId);
                if (dashboard != null)
                {
                    vMashines = this.GetVirtualMachinesByDashboard(dashboard);
                }
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.name = ex.ToString();
                log.timestamp = DateTime.UtcNow;
                context.Logs.Add(log);
                context.SaveChanges();
            }
            return new JsonResult(new { data = vMashines });
        }
        /// <summary>
        /// GetVirtualMachinesByDashboard
        /// </summary>
        /// <param name="_dashboard"></param>
        /// <returns></returns>
        private List<VM> GetVirtualMachinesByDashboard(Dashboard _dashboard)
        {
            List<Application> applications = new List<Application>();
            List<VM> vMashines = new List<VM>();
            try
            {
                if (_dashboard != null)
                {
                    foreach (var apps in _dashboard.DashboardApplications)
                    {
                        if (applications.FirstOrDefault(app => app.AppId == apps.ApplicationId) == null)
                        {
                            applications.Add(context.Applications.FirstOrDefault(a => a.AppId == apps.ApplicationId));
                        }
                    }
                    foreach (var apps in applications)
                    {
                        IAzure azure;
                        try
                        {
                            azure = GetAzureConnection(apps);
                            
                        }
                        catch
                        {
                            continue;
                        }
                        var vmList = azure.VirtualMachines.List();
                        foreach (var _vm in vmList)
                        {
                            VM vm = new VM();
                            vm = createLocalVMFromAzureVM(_vm);
                            vMashines.Add(vm);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log log = new Log();
                log.name = ex.ToString();
                log.timestamp = DateTime.UtcNow;
                context.Logs.Add(log);
                context.SaveChanges();
            }
            return vMashines;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_accessCode"></param>
        /// <returns></returns>
        public VMList GetVirtualMachinesByAccessCode(string _accessCode)
        {
            VMList list = new VMList();
            try
            {
                var dashboard = context.Dashboards.Include(d => d.DashboardApplications).FirstOrDefault(d => d.DashboardAnonAccessCode == _accessCode);
                if (dashboard != null)
                {
                    list.data = this.GetVirtualMachinesByDashboard(dashboard);
                }
            }
            catch
            {
                list.data = new List<VM>();
            }
            return list;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_accessCode"></param>
        /// <returns></returns>
        public Dashboard GetDashboardByAccessCode(string _accessCode)
        {
            var dashboard = context.Dashboards.Include(d => d.DashboardApplications).FirstOrDefault(d => d.DashboardAnonAccessCode == _accessCode);
            return dashboard;
        }
    }
    public class VM
    {
        public string VMName;
        public string ResourceGroupName;
        public string Tags;
        public string SubscriptionId;
        public string VMSize;
        public string PowerState;
        public string ProvisioningState;
        public string VMId;
        public string Action;
    }
    public class VMList
    {
        public List<VM> data;
    }
}