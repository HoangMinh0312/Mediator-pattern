using System;
using System.Collections.Generic;
using System.Text;

namespace LowellMediator.Interfaces
{
    public interface IPostProcessor<TResponse>
    {
        TResponse Process(TResponse response);
    }
}
