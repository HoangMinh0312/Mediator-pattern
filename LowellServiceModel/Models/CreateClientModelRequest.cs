using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowellServiceModel.Models
{
    public class CreateClientModelRequest: IAsyncCommand<CreateClientModelResponse>, AllowGlobalTransaction
    {
        public string Name { get; set; }
    }

    public class CreateClientModelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
