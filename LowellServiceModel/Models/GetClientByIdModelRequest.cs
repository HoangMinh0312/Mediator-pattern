using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LowellServiceModel.Models
{
    public class GetClientByIdModelRequest : IAsyncQuery<GetClientByIdModelResponse>
    {
        public Guid Id { get; set; }
    }

    public class GetClientByIdModelResponse
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
    }
}
