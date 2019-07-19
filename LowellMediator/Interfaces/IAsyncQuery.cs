using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LowellMediator.Interfaces
{
    public interface IAsyncQuery<out TRespnose>
    {

    }

    public interface IAsyncQueryHandler<in TRequest, TResponse>
        where TRequest : IAsyncQuery<TResponse>
    {
        Task<TResponse> Handle(TRequest request);
    }
}
