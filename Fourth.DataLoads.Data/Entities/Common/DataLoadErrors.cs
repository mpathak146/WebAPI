using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Fourth.DataLoads.Data.Entities
{
    [Table("t_DataLoadErrors")]
    public class DataLoadErrors
    {
        public long DataLoadErrorsId { get; set; }
        [Column(TypeName = "xml")]
        public string ErrRecord { get; set; }
        public string ErrDescription { get; set; }
        public long DataLoadBatchRefId { get; set; }
        [ForeignKey("DataLoadBatchRefId")]
        public virtual DataLoadBatch DataLoadBatch { get; set; }

    }
}
