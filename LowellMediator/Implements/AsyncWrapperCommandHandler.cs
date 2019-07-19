using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace LowellMediator.Implements
{
    public abstract class AsyncWrapperCommandHandler<TResponse>
    {
        public abstract Task<TResponse> Handle(IAsyncCommand<TResponse> message);
    }

    public class AsyncWrapperCommandHandler<TRequest, TResponse> : AsyncWrapperCommandHandler<TResponse> where TRequest : IAsyncCommand<TResponse>
    {
        private readonly IAsyncCommandHandler<TRequest, TResponse> _innerCommandHandler;
        private readonly IPreProcessor<TRequest> _preProcessor;
        private readonly IPostProcessor<TResponse> _postProcessor;

        public AsyncWrapperCommandHandler(IAsyncCommandHandler<TRequest, TResponse> innerCommandHandler,
            IPreProcessor<TRequest> preProcessor = null,
            IPostProcessor<TResponse> postProcessor = null)
        {
            _innerCommandHandler = innerCommandHandler;
            _preProcessor = preProcessor;
            _postProcessor = postProcessor;
        }

        public async override Task<TResponse> Handle(IAsyncCommand<TResponse> message)
        {
            if (_preProcessor != null)
            {
                message = _preProcessor.Process((TRequest)message);
            }

            TResponse response;

            if (typeof(AllowGlobalTransaction).IsAssignableFrom(typeof(TRequest)))
            {
                using (var transaction = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
                {
                    response = await _innerCommandHandler.Handle((TRequest)message);
                    transaction.Complete();
                }
            }
            else
            {
                response = await _innerCommandHandler.Handle((TRequest)message);
            }

            if (_postProcessor != null)
            {
                response = _postProcessor.Process(response);
            }

            return response;
        }
    }
}
