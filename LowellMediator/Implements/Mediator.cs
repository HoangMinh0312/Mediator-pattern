using LowellMediator.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace LowellMediator.Implements
{
    public delegate object InnerInstanceFactory(Type serviceType);
    public delegate object PreInstanceFactory(Type serviceType);
    public delegate object PostInstanceFactory(Type serviceType);


    public class Mediator : IMediator
    {
        private readonly InnerInstanceFactory _innerFactory;
        private readonly PreInstanceFactory _preFactory;
        private readonly PostInstanceFactory _postFactory;

        public Mediator(InnerInstanceFactory innerFactory, PreInstanceFactory preFactory, PostInstanceFactory postFactory)
        {
            _innerFactory = innerFactory;
            _preFactory = preFactory;
            _postFactory = postFactory;
        }

        public Task<TResponse> SendAsync<TResponse>(IAsyncCommand<TResponse> request)
        {
            var wrapper = GetWrapper<AsyncWrapperCommandHandler<TResponse>, TResponse>(request, typeof(IAsyncCommandHandler<,>), typeof(AsyncWrapperCommandHandler<,>));
            return wrapper.Handle(request);
        }

        public Task<TResponse> SendAsync<TResponse>(IAsyncQuery<TResponse> request)
        {
            var wrapper = GetWrapper<AsyncWrapperQueryHandler<TResponse>, TResponse>(request, typeof(IAsyncQueryHandler<,>), typeof(AsyncWrapperQueryHandler<,>));
            return wrapper.Handle(request);
        }

        private TWrapper GetWrapper<TWrapper, TResponse>(object request, Type basehandlerType, Type typeBaseWrapper)
        {
            var requestType = request.GetType();
            var handlerType = basehandlerType.MakeGenericType(requestType, typeof(TResponse));
            var handler = GetHandler(request, handlerType);

            var wrapperType = typeBaseWrapper.MakeGenericType(requestType, typeof(TResponse));
            var preProcessors = _preFactory.Invoke(typeof(IPreProcessor<>)?.MakeGenericType(requestType));
            var postProcessors =
                _postFactory.Invoke(typeof(IPostProcessor<>)?.MakeGenericType(typeof(TResponse)));
            var wrapper = (TWrapper)Activator.CreateInstance(wrapperType, handler, preProcessors, postProcessors);
            return wrapper;

        }

        private object GetHandler(object request, Type handlerType)
        {
            try
            {
                return _innerFactory(handlerType);
            }
            catch (Exception e)
            {
                throw new Exception("Handler not found");
            }
        }
    }
}
