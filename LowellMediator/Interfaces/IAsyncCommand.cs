using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LowellMediator.Interfaces
{

    public interface IAsyncCommand<out TResponse>
    {
    }

    public interface IAsyncCommandHandler<in TRequest, TResponse>
        where TRequest : IAsyncCommand<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
