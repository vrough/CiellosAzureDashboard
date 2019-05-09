using CiellosAzureDashboard.Data;
using CiellosAzureDashboard.Model;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Threading;

namespace CiellosAzureDashboard
{
    public class Program
    {
        private static CancellationTokenSource cancelTokenSource = new System.Threading.CancellationTokenSource();

        public static void Main(string[] args)
        {

            bool trick = true;
            if (trick)
            {
                var host = CreateWebHostBuilder(args).Build();

                using (var scope = host.Services.CreateScope())
                {
                    var services = scope.ServiceProvider;

                    try
                    {
                        var context = services.GetRequiredService<CADContext>();
                        context.Database.Migrate();
                        context.Database.EnsureCreated();
                        if (!context.Users.Any())
                        {
                            DBInitializer.Initialize(context);
                        }

                    }
                    catch (Exception ex)
                    {
                        var logger = services.GetRequiredService<ILogger<Program>>();
                        logger.LogError(ex, "An error occurred creating the DB.");
                    }
                }
                host.RunAsync(cancelTokenSource.Token).GetAwaiter().GetResult();
            }
            else
            {
                var host = CreateWebHostBuilderWizard(args).Build();
                host.Run();
            }

        }

        public static void Shutdown()
        {
            cancelTokenSource.Cancel();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
        public static IWebHostBuilder CreateWebHostBuilderWizard(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<StartupWizard>();
    }
}
