using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LowellMediator.Implements
{
    public abstract class AsyncWrapperQueryHandler<TResponse>
    {
        public abstract Task<TResponse> Handle(IAsyncQuery<TResponse> message);
    }

    public class AsyncWrapperQueryHandler<TRequest, TResponse> : AsyncWrapperQueryHandler<TResponse> where TRequest : IAsyncQuery<TResponse>
    {
        private readonly IAsyncQueryHandler<TRequest, TResponse> _innerAsyncQueryHandler;
        private readonly IEnumerable<IPreProcessor<TRequest>> _preProcessor;
        private readonly IEnumerable<IPostProcessor<TResponse>> _postProcessor;

        public AsyncWrapperQueryHandler(IAsyncQueryHandler<TRequest, TResponse> innerAsyncQueryHandler,
            IEnumerable<IPreProcessor<TRequest>> preProcessor = null,
            IEnumerable<IPostProcessor<TResponse>> postProcessor = null)
        {
            _innerAsyncQueryHandler = innerAsyncQueryHandler;
            _preProcessor = preProcessor;
            _postProcessor = postProcessor;
        }

        public async override Task<TResponse> Handle(IAsyncQuery<TResponse> message)
        {

            if (_preProcessor != null)
            {
                foreach (var preProcessor in _preProcessor)
                {
                    message = preProcessor.Process((TRequest)message);
                }
            }
            TResponse response;


            response = await _innerAsyncQueryHandler.Handle((TRequest)message);


            if (_postProcessor != null)
            {
                foreach (var postProcessor in _postProcessor)
                {
                    response = postProcessor.Process(response);
                }
            }

            return response;
        }
    }

}
