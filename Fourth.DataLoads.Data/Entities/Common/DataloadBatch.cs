namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("t_DataLoadBatch")]
    public partial class DataLoadBatch
    {
        public DataLoadBatch()
        {
            this.MassesToTerminate = new HashSet<MassTermination>();
        }
        [Key]
        [Column(Order = 0)]
        public long DataLoadJobId { get; set; }
        [Key]
        [Column(Order = 1)]
        public long DataLoadBatchId { get; set; }
        [Column(Order = 2)]
        public long DataloadTypeRefID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ?DateProcessed { get; set; }
        [Column(Order = 3)]
        public int GroupID { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        [ForeignKey("DataloadTypeRefID")]
        public virtual DataLoadType DataLoadType { get; set; }
        public virtual ICollection<MassTermination> MassesToTerminate { get; set; }
        public virtual ICollection<DataLoadErrors> dataloadErrors { get; set; }   

    }
}
