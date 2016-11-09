using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fourth.DataLoads.ApiEndPoint
{
    public static class Constants
    {
        private const string headername = "X-Fourth-Org";
        
        public static string HeaderName
        {
            get
            {
                return headername;
            }
        }
    }
}