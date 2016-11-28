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
            Batches = new List<DataLoadBatch>();
        }
        [DatabaseGeneratedAttribute(DatabaseGeneratedOption.None)]
        public long DataloadTypeID { get; set; }
        public string DataloadType { get; set; }
        internal virtual ICollection<DataLoadBatch> Batches { get; set; }
    }
}
