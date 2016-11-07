namespace Fourth.DataLoads.Data.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("t_HOL_DefaultAllowance")]
    public class DefaultHolidayAllowanceTable
    {
        [Key]
        [Column(Order = 1)]
        public int JobTitleID { get; set; }

        [Key]
        [Column(Order = 2)]
        public int YearsWorked { get; set; }

        public int Allowance { get; set; }
        public System.DateTime LastModified { get; set; }
        public System.DateTime DateCreated { get; set; }

        public virtual JobTitleTable JobTitleTable { get; set; }
    }
}