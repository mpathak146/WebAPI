using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;

namespace Fourth.DataLoads.ApiEndPoint.Mappers
{
    public interface IMappingFactory
    {
        IMapper Mapper { get; }
    }
}
