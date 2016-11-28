namespace Fourth.DataLoads.Data.Entities
{
    using Interface;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    public partial class MassTerminationModel:IModelMarker
    {
        public long Id { get; set; }
        public long DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public virtual DataLoadBatch DataLoadBatch { get; set; }
        public string ErrValidation { get; set; }
    }
}
