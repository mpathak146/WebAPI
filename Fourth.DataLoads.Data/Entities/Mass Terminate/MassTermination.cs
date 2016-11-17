namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("t_MassTermination")]
    public partial class MassTermination
    {
        public long Id { get; set; }
        public long DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public virtual DataLoadBatch DataLoadBatch { get; set; } 
    }
}
