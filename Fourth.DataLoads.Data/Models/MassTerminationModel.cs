namespace Fourth.DataLoads.Data.Entities
{
    using System;

    public partial class MassTerminationModel
    {
        public long DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string ErrValidation { get; set; }

    }
    [Serializable]
    public partial class MassTerminationModelSerialized 
    {
        public long DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string ErrValidation { get; set; }
    }
}
