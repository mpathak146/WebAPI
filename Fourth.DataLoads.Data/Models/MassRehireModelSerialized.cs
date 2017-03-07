using Fourth.DataLoads.Data.Interfaces;
using System;

namespace Fourth.DataLoads.Data.Entities
{

    public class MassRehireModel
    {
        public string EmployeeNumber { get; set; }
    }
    [Serializable]
    public class MassRehireModelSerialized : IMarker
    {
        public string EmployeeNumber { get; set; }

    }

}