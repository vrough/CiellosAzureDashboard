using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace CiellosAzureDashboard.Model
{
    public class Setting
    {
        [Key]
        public int settingId { get; set; }
        [DisplayName("Certificate Thumbprint")]
        public string certificateThumbprintStr { get; set; }
        [DisplayName("Maximum nummer of events for store VM logs(per VM)")]

        public int MaxNumEventsLogStorePerVM { get; set; }
    }
}
