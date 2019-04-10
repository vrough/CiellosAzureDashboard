using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class Link
    {
        [Key]
        public int linkId { get; set; }
        [DisplayName("Name")]
        public string linkName { get; set; }
        [DisplayName("URL")]
        public string linkUrl { get; set; }
        public int DashboardId { get; set; }
        public virtual Dashboard Dashboard { get; set; }
    }
}
