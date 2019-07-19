using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowellServiceModel.Models
{
    public class UpdateClientModelRequest: IAsyncCommand<UpdateClientModelResponse>
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }

    public class UpdateClientModelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
