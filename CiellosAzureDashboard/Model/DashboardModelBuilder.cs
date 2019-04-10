using Microsoft.AspNet.OData.Builder;
using Microsoft.OData.Edm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class DashboardModelBuilder
    {
        public IEdmModel GetEdmModel(IServiceProvider sProvider)
        {
            ODataConventionModelBuilder builder = new ODataConventionModelBuilder(sProvider);
            builder.EntitySet<Application>("Applications");
            builder.EntitySet<Dashboard>("Dashboards");
            return builder.GetEdmModel();
        }
    }
}
