using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LowellMediator.Interfaces
{
    public interface IMediator
    {
        Task<TResponse> SendAsync<TResponse>(IAsyncCommand<TResponse> request);
        Task<TResponse> SendAsync<TResponse>(IAsyncQuery<TResponse> request);
    }
}
