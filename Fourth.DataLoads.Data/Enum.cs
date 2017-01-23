using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Data
{
    public enum DataLoadTypes
    {        
        MASS_TERMINATION=1,
        CASUAL_HOLIDAY=2,
        HOLIDAY_DAYSTAKEN=3,
        HOLIDAY_HOURSTAKEN=4,
        PAYROLL_STARTDATE=5,
        SALARY_INCREASE=6,
        MASS_REHIRE=7
    };
    public enum DataloadStatus
    {
        STAGING_DB_UPDATED,
        REQUEST_RECEIVED,
        BATCH_PROCESSED,
        PARTIALLYPROCESSED,
    }
    
}
