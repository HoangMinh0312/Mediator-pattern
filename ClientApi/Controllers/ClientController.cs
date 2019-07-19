using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LowellServiceModel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Service.Clients;

namespace ClientApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClientController : ControllerBase
    {
        private IClientService _clientService;

        public ClientController(IClientService clientService)
        {
            _clientService = clientService;
        }

        [HttpGet]
        public async Task<GetClientByIdModelResponse> Get(Guid id)
        {
            return await _clientService.Get(new GetClientByIdModelRequest { Id=id});
        }

        [HttpPost]
        public async Task<CreateClientModelResponse> Create(CreateClientModelRequest model)
        {
            return await _clientService.Create(model);
        }

        [HttpPut]
        public async Task<UpdateClientModelResponse> Update(UpdateClientModelRequest model)
        {
            return await _clientService.Update(model);
        }

        [HttpDelete]
        public async Task<DeleteClientModelResponse> Delete(DeleteClientModelRequest model)
        {
            return await _clientService.Delete(model);
        }
    }
}