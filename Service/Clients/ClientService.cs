using LowellMediator.Interfaces;
using LowellServiceModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Service.Clients
{
    public class ClientService : IClientService
    {
        private readonly IMediator _mediator;

        public ClientService(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task<CreateClientModelResponse> Create(CreateClientModelRequest model)
        {
            return await _mediator.SendAsync(model);
        }

        public async Task<DeleteClientModelResponse> Delete(DeleteClientModelRequest model)
        {
            return await _mediator.SendAsync(model);
        }

        public async Task<GetClientByIdModelResponse> Get(GetClientByIdModelRequest model)
        {
            return await _mediator.SendAsync(model);

        }

        public async Task<UpdateClientModelResponse> Update(UpdateClientModelRequest model)
        {
            return await _mediator.SendAsync(model);
        }
    }
}
