using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data
{
    public enum DataLoadTypes
    {
        All=1,
        MASS_TERMINATION=2,
        CASUAL_HOLIDAY=3,
        HOLIDAY_DAYSTAKEN=4,
        HOLIDAY_HOURSTAKEN=5,
        PAYROLL_STARTDATE=6,
        SALARY_INCREASE=7,
        MASS_REHIRE=8
    };
    public enum DataloadStatus
    {
        STAGING_DB_UPDATED,
        REQUEST_RECEIVED,
        BATCH_PROCESSED,
        PARTIALLYPROCESSED,
    }
    
}
