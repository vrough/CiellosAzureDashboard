using CiellosAzureDashboard.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class DBInitializer
    {
        public static void Initialize(CADContext context)
        {
            Application app = new Application();
            app.ClientId = "912fabb6-a061-4286-a476-aba6e7edb0e3";
            app.ClientSecret = "6LGURivA4/gRNUzjRGuvWPZxKnwmESxOgdx1G+5EZgY=";
            app.SubscriptionId = "1d52c7db-0f59-4b3e-artd-de3df0624ab1";
            app.TenantId = "test.com";
            app.Description = "test subscription";
            context.Applications.Add(app);
            context.SaveChanges();

            Dashboard dashBoard = new Dashboard();
            dashBoard.DashboardName = "DemoTestDashboard";
            dashBoard.DashboardAnonAccessCode = "testtesttest";
            dashBoard.DashboardLogoUrl = "https://ciellospublic.z21.web.core.windows.net/Ciellos Logo_Standard.png";

            context.Dashboards.Add(dashBoard);
            context.SaveChanges();

            User user = new User();
            user.UserName = "test.test@test.com";
            user.IsSuperUser = false;
            context.Users.Add(user);
            context.SaveChanges();

            Setting setting = new Setting();
            setting.MaxNumEventsLogStorePerVM = 1;
            context.Settings.Add(setting);
            context.SaveChanges();
         }
    }
}
