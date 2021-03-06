﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.Spatial;
namespace Fourth.DataLoads.Data.Entities
{
    [Table("t_DataLoadErrors")]
    public class DataLoadErrors
    {
        [Column(Order = 0)]
        public long DataLoadErrorsId { get; set; }
        [ForeignKey("DataLoadBatch"), Column(Order = 1)]
        public Guid DataLoadJobRefId { get; set; }
        [ForeignKey("DataLoadBatch"), Column(Order = 2)]
        public Guid DataLoadBatchRefId { get; set; }
        [Column(TypeName = "xml")]
        public string ErrRecord { get; set; }
        public string ErrDescription { get; set; }
        public virtual DataLoad DataLoadBatch { get; set; }

    }
}
