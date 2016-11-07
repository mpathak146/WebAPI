using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Models
{
    public class MassTerminate
    {
        public string EmployeeNumber { get; set; }
        public DateTime TerminationDate { get; set; }
        public string TerminationReason { get; set; }
    }
}
