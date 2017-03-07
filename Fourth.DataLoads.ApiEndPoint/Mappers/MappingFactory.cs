using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Fourth.DataLoads.ApiEndPoint.Mappers
{
    public class MappingFactory : IMappingFactory
    {
        private readonly IMapper _mappingInstance;

        public MappingFactory()
        {
            // Define all the mappings here
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Data.Entities.MassTerminationModelSerialized, 
                    Data.Entities.MassTerminationModel>();
                cfg.CreateMap<Data.Entities.MassTerminationModel,
                    Data.Entities.MassTerminationModelSerialized>();
                cfg.CreateMap<Data.Entities.MassRehireModelSerialized,
                    Data.Entities.MassRehireModel>();
                cfg.CreateMap<Data.Entities.MassRehireModel,
                    Data.Entities.MassRehireModelSerialized>();

                //Create more maps as needed
            });

            _mappingInstance = config.CreateMapper();
        }
        public IMapper Mapper
        {
            get
            {
                return this._mappingInstance;
            }
        }
    }
}