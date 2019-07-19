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
    public class DeleteClientCommandHandler : IAsyncCommandHandler<DeleteClientModelRequest, DeleteClientModelResponse>
    {
        private readonly IConfiguration _configuration;

        public DeleteClientCommandHandler(IConfiguration configuration)
        {
            this._configuration = configuration;
        }
        public async Task<DeleteClientModelResponse> Handle(DeleteClientModelRequest request)
        {
            var connectionString = this.GetConnection();
            var count = 0;

            using (var con = new SqlConnection(connectionString))
            {
                try
                {
                    con.Open();
                    var query = "DELETE FROM CLIENT WHERE Id = @Id";
                    count = await con.ExecuteAsync(query, new { Id = request.ClientId });

                    if (count > 0)
                    {
                        return new DeleteClientModelResponse { };
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

                return null;
            }
        }

        private string GetConnection()
        {
            var connection = _configuration.GetSection("ConnectionString").Value;
            return connection;
        }
    }
}
