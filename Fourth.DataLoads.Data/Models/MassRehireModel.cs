using Fourth.DataLoads.Data.Interfaces;
using System;

namespace Fourth.DataLoads.Data.Entities
{

    public class MassRehireModel
    {
        public string EmployeeNumber { get; set; }
        public string RehireDate { get; set; }

    }
    [Serializable]
    public class MassRehireModelSerialized : IMarker
    {
        public Guid DataLoadJobId { get; set; }
        public Guid DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string RehireDate { get; set; }
        public string ErrValidation { get; set; }
    }

}