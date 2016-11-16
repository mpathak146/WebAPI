namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("MassTermination")]
    public partial class MassTermination
    {
        public long Id { get; set; }
        public string BatchID { get; set; }
        public string EmployeeNumber { get; set; }
        public DateTime TerminationDate { get; set; }
        public string TerminationReason { get; set; }
    }
}
