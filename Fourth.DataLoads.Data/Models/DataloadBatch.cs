using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Models
{
    public class DataloadBatch
    {
        public Guid JobID { get; set; }
        public Guid BatchID { get; set; }
        public string User { get; set; }
        public string OrganizationID { get; set; }
    }
}
