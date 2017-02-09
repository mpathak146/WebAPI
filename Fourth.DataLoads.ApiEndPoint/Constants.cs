using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Fourth.DataLoads.ApiEndPoint
{
    public static class Constants
    {
        private const string headerOrg = "x-fourth-org";
        private const string headerUserId = "x-fourth-userid";

        public static string HeaderOrg
        {
            get
            {
                return headerOrg;
            }
        }
        public static string HeaderUser
        {
            get
            {
                return headerUserId;
            }
        }
    }
}