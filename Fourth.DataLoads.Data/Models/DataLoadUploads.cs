using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data.Models
{
    public class DataLoadUploads
    {
        public string jobID { get; set; }
        public string DataloadType { get; set; }
        public DateTime DateUploaded { get; set; }
        public string UploadedBy { get; set; }
    }

}
