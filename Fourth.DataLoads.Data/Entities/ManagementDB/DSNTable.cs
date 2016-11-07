namespace Fourth.DataLoads.Data.Entities.ManagementDB
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("t_dsn")]
    public class DSNTable
    {
        [Key]
        public int GroupId { get; set; }

        public string AppName { get; set; }
        public string DataSource { get; set; }
        public string SchemaName { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public int? PIN { get; set; }
    }
}