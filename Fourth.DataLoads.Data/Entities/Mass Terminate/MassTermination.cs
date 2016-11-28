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
        public long DataLoadBatchRefId { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        [ForeignKey("DataLoadBatchRefId")]
        public virtual DataLoadBatch DataLoadBatch { get; set; } 
    }
}
