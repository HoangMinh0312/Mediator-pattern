using Dapper;
using LowellMediator.Interfaces;
using LowellServiceModel.Entities;
using LowellServiceModel.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lowell.Query.Clients
{
    public class GetClientByIdModelRequestHandler : IAsyncQueryHandler<GetClientByIdModelRequest, GetClientByIdModelResponse>
    {
        private readonly IConfiguration _configuration;

        public GetClientByIdModelRequestHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<GetClientByIdModelResponse> Handle(GetClientByIdModelRequest request)
        {
            var connectionString = this.GetConnection();
            GetClientByIdModelResponse response = default(GetClientByIdModelResponse);

            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "SELECT * FROM Client WHERE Id = @Id";
                    var client = (await con.QueryAsync<Client>(query,new { Id= request .Id})).FirstOrDefault();

                    if (client != null)
                    {
                        return new GetClientByIdModelResponse
                        {
                            Id = client.Id,
                            Name = client.Name
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
                return new GetClientByIdModelResponse();
            }
        }

        private string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionString").Value;
            return connection;
        }
    }
}
