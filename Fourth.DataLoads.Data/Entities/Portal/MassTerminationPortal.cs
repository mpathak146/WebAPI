namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    [Table("t_MassTermination")]
    public partial class MassTerminationPortal
    {
        public long MassTerminationId { get; set; }
        public string DataLoadJobRefId { get; set; }
        public string DataLoadBatchRefId { get; set; }
        public int ClientID { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public int ErrorStatus { get; set; }
        public string ErrorDescription { get; set; }

    }
}