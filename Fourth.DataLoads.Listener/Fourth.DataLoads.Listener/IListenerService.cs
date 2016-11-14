using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Fourth.DataLoads.Listener
{
    interface IListenerService: IDisposable
    {
        /// <summary>
        /// Allows the service to be started when running as a console application.
        /// </summary>
        /// <param name="args"></param>
        void StartService(string[] args);

        /// <summary>
        /// Allows the service to be stopped when running as a console application.
        /// </summary>
        /// <param name="args"></param>
        void StopService();
    }
}
