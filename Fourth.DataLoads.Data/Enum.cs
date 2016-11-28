using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data
{
    public enum DataLoadTypes
    {
        MassTermination=1,
        CasualHoliday=2,
        HolidayDaysTaken=3,
        HolidayHoursTaken=4,
        PayrollStartDate=5,
        SalaryIncrease=6
    };
    public enum DataloadStatus
    {
        Requested,
        Processed,
        PartiallyProcessed,
    }
    
}
