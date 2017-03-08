namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;
    [Table("t_MassRehire")]
    public partial class MassRehire
    {
        [Column(Order = 0)]
        public long MassRehireId { get; set; }

        [ForeignKey("DataLoadBatch"), Column(Order = 1)]
        public Guid DataLoadJobRefId { get; set; }
        [ForeignKey("DataLoadBatch"), Column(Order = 2)]
        public Guid DataLoadBatchRefId { get; set; }
        [Column(Order = 3)]
        public string EmployeeNumber { get; set; }
        [Column(Order = 4)]
        public DateTime RehireDate { get; set; }
        [Column(Order = 5)]
        public virtual DataLoad DataLoadBatch { get; set; }
    }
}
