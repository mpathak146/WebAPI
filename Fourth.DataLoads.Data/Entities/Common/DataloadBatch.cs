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
        public long DataLoadBatchId { get; set; }
        public long DataloadTypeID { get; set; }
        public int GroupID { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime ?DateProcessed { get; set; }
        public string Status { get; set; }
        public string UserName { get; set; }
        protected virtual DataLoadType DataLoadType { get; set; }
        protected virtual ICollection<MassTermination> MassesToTerminate { get; set; }

    }
}
