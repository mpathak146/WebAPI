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
        [Column(Order = 0)]
        public long MassTerminationId { get; set; }

        [ForeignKey("DataLoadBatch"), Column(Order = 1)]
        public long DataLoadJobRefId { get; set; }
        [ForeignKey("DataLoadBatch"), Column(Order = 2)]
        public long DataLoadBatchRefId { get; set; }
        [Column(Order = 3)]
        public string EmployeeNumber { get; set; }
        [Column(Order = 4)]
        public DateTime TerminationDate { get; set; }
        [Column(Order = 5)]
        public string TerminationReason { get; set; }
        public virtual DataLoadBatch DataLoadBatch { get; set; } 
    }
}
