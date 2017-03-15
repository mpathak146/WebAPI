using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.ProtocolBuffers;
using Fourth.DataLoads.Data.Models;
using Fourth.DataLoads.Data.Repositories;
using Fourth.DataLoads.Data.Interfaces;
using Fourth.DataLoads.Data.Entities;

namespace Fourth.DataLoads.Listener.Handlers
{
    public interface IMassRehireService<TMessage>
                                    where TMessage : IMessage
    {
        Task<bool> ProcessPayload(TMessage payload, 
            IDataFactory dataFactory);
    }
}
