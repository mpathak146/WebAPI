namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("t_DataLoadType")]
    public partial class DataLoadType
    {
        public DataLoadType()
        {
            this.Batches = new HashSet<DataLoadBatch>();
        }
        public long DataloadTypeID { get; set; }
        public string DataloadType { get; set; }

        public virtual ICollection<DataLoadBatch> Batches { get; set; }
    }
}
