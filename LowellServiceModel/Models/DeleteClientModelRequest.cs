using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowellServiceModel.Models
{
    public class DeleteClientModelRequest: IAsyncCommand<DeleteClientModelResponse>
    {
        public Guid ClientId { get; set; }
    }

    public class DeleteClientModelResponse
    {

    }
}
