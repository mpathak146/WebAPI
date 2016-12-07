namespace Fourth.DataLoads.Data.Entities
{
    using Interfaces;
    using System;

    public partial class MassTerminationModel
    {
        public Guid DataLoadJobId { get; set; }
        public Guid DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string ErrValidation { get; set; }

    }
    [Serializable]
    public partial class MassTerminationModelSerialized : IMarker
    {
        public Guid DataLoadJobId { get; set; }
        public Guid DataLoadBatchId { get; set; }
        public string EmployeeNumber { get; set; }
        public string TerminationDate { get; set; }
        public string TerminationReason { get; set; }
        public string ErrValidation { get; set; }
    }
}
