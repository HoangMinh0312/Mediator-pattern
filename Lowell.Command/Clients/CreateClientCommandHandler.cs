using LowellServiceModel.Models;
using System;
using System.Collections.Generic;
using System.Text;
using LowellMediator.Interfaces;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using Dapper;

namespace Lowell.Command.Clients
{
    public class PreCreateClientCommandHandler : IPreProcessor<CreateClientModelRequest>
    {
        public CreateClientModelRequest Process(CreateClientModelRequest request)
        {
            if (request.Name.Equals("abc"))
            {
                throw new Exception("asdasdas");
            }
            return request;
        }
    }


    public class CreateClientCommandHandler : IAsyncCommandHandler<CreateClientModelRequest, CreateClientModelResponse>
    {
        private readonly IConfiguration _configuration;

        public CreateClientCommandHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<CreateClientModelResponse> Handle(CreateClientModelRequest request)
        {
            var connectionString = this.GetConnection();
            var response = default(CreateClientModelResponse);
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    var newId = Guid.NewGuid();
                    con.Open();
                    var query = "INSERT INTO Client(Id, Name) VALUES(@Id, @Name)";
                    int result = await con.ExecuteAsync(query, new { Id = newId, request.Name });

                    if (result > 0)
                    {
                        response = new CreateClientModelResponse
                        {
                            Id = newId,
                            Name = request.Name
                        };
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    con.Close();
                }
            }

            return response;
        }

        private string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionString").Value;
            return connection;
        }
    }

    public class PostCreateClientCommandHandler : IPostProcessor<CreateClientModelResponse>
    {
        public CreateClientModelResponse Process(CreateClientModelResponse response)
        {
            //Notify to another service to do sth
            return response;
        }
    }
}
