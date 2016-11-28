namespace Fourth.DataLoads.Data.Entities
{
    using Interface;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    using System.Runtime.Serialization;
    
    public partial class MassTerminationModel:IModelMarker
    {
        public long DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string ErrValidation { get; set; }

    }
    [Serializable]
    public partial class MassTerminationModelSerialized : IModelMarker
    {
        public long DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string ErrValidation { get; set; }
    }
}
