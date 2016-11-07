namespace Fourth.DataLoads.Data.Entities
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("t_JobTitle")]
    public class JobTitleTable
    {
        [Key]
        public int JobTitleID { get; set; }

        public string Name { get; set; }
        public Nullable<int> Priority { get; set; }
        public System.DateTime DateCreated { get; set; }
        public System.DateTime LastModified { get; set; }
        public bool RecordStatus { get; set; }
        public string Code { get; set; }
        public int GradeID { get; set; }
        public int RotaSetting { get; set; }
        public int StageID { get; set; }
        public Nullable<int> Headcount { get; set; }
        public Nullable<bool> CreativeLearning { get; set; }
        public Nullable<int> CreativeJobID { get; set; }
        public Nullable<double> ContractHours { get; set; }
        public Nullable<bool> LinkRates { get; set; }
        public Nullable<double> HourlyMinimum { get; set; }
        public Nullable<double> HourlyMaximum { get; set; }
        public Nullable<double> ShiftMinimum { get; set; }
        public Nullable<double> ShiftMaximum { get; set; }
        public Nullable<double> SalaryMinimum { get; set; }
        public Nullable<double> SalaryMaximum { get; set; }
        public Nullable<double> HourlyMinimumUnder21 { get; set; }
        public Nullable<double> HourlyMaximumUnder21 { get; set; }
        public Nullable<double> ShiftMinimumUnder21 { get; set; }
        public Nullable<double> ShiftMaximumUnder21 { get; set; }
        public Nullable<double> SalaryMinimumUnder21 { get; set; }
        public Nullable<double> SalaryMaximumUnder21 { get; set; }
        public Nullable<bool> IsContractor { get; set; }
        public Nullable<bool> IsExcludeFromWageCost { get; set; }
        public Nullable<bool> UseInRecruitment { get; set; }
        public Nullable<int> RotaPayRate { get; set; }
        public bool ExcludeFromTronc { get; set; }
        public bool DisplayAdditionalPayment1 { get; set; }
        public bool DisplayAdditionalPayment2 { get; set; }
        public int InclJobTitleInRota { get; set; }
        public Nullable<int> ExcludeFromTips { get; set; }
        public Nullable<bool> IsSupervisor { get; set; }
        public Nullable<bool> ExcludeFromMetrics { get; set; }
        public Nullable<int> JZeroJobID { get; set; }
        public Nullable<bool> JZero { get; set; }
        public Nullable<double> HourlyMinimumUnder18 { get; set; }
        public Nullable<double> HourlyMaximumUnder18 { get; set; }
        public Nullable<double> ShiftMinimumUnder18 { get; set; }
        public Nullable<double> ShiftMaximumUnder18 { get; set; }
        public bool AgencyStaffExcludeFromNI { get; set; }
        public bool GetExtraPayOverThresholdHours { get; set; }
        public double ThresholdHours { get; set; }
        public decimal ExtraPayment { get; set; }
        public string JobCategoryID { get; set; }
        public Nullable<int> DeductionType { get; set; }
        public string AccommodationType { get; set; }
        public Nullable<bool> IsSystemUser { get; set; }
        public System.Guid JobTitleGUID { get; set; }
        public Nullable<double> HourlyMinimumApprentice { get; set; }
        public Nullable<double> HourlyMaximumApprentice { get; set; }
        public Nullable<double> ShiftMinimumApprentice { get; set; }
        public Nullable<double> ShiftMaximumApprentice { get; set; }
        public Nullable<double> SalaryMinimumApprentice { get; set; }
        public Nullable<double> SalaryMaximumApprentice { get; set; }
        public Nullable<double> HourlyMinimumOver25 { get; set; }
        public Nullable<double> HourlyMaximumOver25 { get; set; }
        public Nullable<double> ShiftMinimumOver25 { get; set; }
        public Nullable<double> ShiftMaximumOver25 { get; set; }
        public Nullable<double> SalaryMinimumOver25 { get; set; }
        public Nullable<double> SalaryMaximumOver25 { get; set; }
        public Nullable<double> SalaryMinimumUnder18 { get; set; }
        public Nullable<double> SalaryMaximumUnder18 { get; set; }
        public Nullable<bool> AllowApprentices { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DefaultHolidayAllowanceTable> DefaultAllowanceTable { get; set; }
    }
}