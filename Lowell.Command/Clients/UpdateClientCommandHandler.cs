using Dapper;
using LowellMediator.Interfaces;
using LowellServiceModel.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Lowell.Command.Clients
{
    public class UpdateClientCommandHandler : IAsyncCommandHandler<UpdateClientModelRequest, UpdateClientModelResponse>
    {
        private readonly IConfiguration _configuration;

        public UpdateClientCommandHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }

        public async Task<UpdateClientModelResponse> Handle(UpdateClientModelRequest request)
        {
            var connectionString = this.GetConnection();
            var response = default(UpdateClientModelResponse);
            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "UPDATE Client SET Name=@Name WHERE Id = @Id";
                    int result = await con.ExecuteAsync(query, new { Id = request.Id, request.Name });

                    if (result > 0)
                    {
                        response = new UpdateClientModelResponse
                        {
                            Id = request.Id,
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
}
