using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using LowellServiceModel.Models;
namespace Service.Clients
{
    public interface IClientService
    {
        Task<CreateClientModelResponse> Create(CreateClientModelRequest model);
        Task<UpdateClientModelResponse> Update(UpdateClientModelRequest model);
        Task<DeleteClientModelResponse> Delete(DeleteClientModelRequest model);
        Task<GetClientByIdModelResponse> Get(GetClientByIdModelRequest model);
    }
}
