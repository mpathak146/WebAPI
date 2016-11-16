using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleApplication1
{
    class Program
    {
        private const string CONNECTION_STRING_FORMAT = "Server={0};Database={1};User ID={2};Password={3};Connection Timeout=30;MultipleActiveResultSets=True;persist security info=True;";

        static void Main(string[] args)
        {
            string _ConnectionString;
            string file = @"C:\Windows\SysWOW64\TRGAdvantage\TRGSettings.xml";
            XDocument xdoc = XDocument.Load(file);
            var dsns = from dsn in xdoc.Elements("Settings").Elements("DSNs").Elements("DSN")
                       where dsn.Attribute("GroupID").Value == "426"
                       select new
                       {
                           UserName = dsn.Attribute("Username").Value,
                           Password = dsn.Attribute("Password").Value,
                           DataSource = dsn.Attribute("DataSource").Value,
                           SchemaName = dsn.Attribute("SchemaName").Value
                       };



            foreach (var dsn in dsns)
            {
                _ConnectionString = string.Format(CONNECTION_STRING_FORMAT, dsn.DataSource, dsn.SchemaName, dsn.UserName, dsn.Password);
            }
        }
    }
}
