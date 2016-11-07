namespace Fourth.DataLoads.Data.Models
{
    using System;

    /// <summary>
    /// For working with Default Holiday Allowances
    /// </summary>
    public class DefaultHolidayAllowance
    {
        public string JobTitleName
        { get; set; }

        public int JobTitleID
        { get; set; }

        public int YearsWorked
        { get; set; }

        public int Allowance
        { get; set; }

        public DateTime LastModified
        { get; set; }

        public DateTime DateCreated
        { get; set; }
    }
}