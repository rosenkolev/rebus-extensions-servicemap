using System.Threading;
using System.Threading.Tasks;

using Rebus.Handlers;
using Rebus.Pipeline;

namespace Rebus.Extensions.ServiceMap
{
    /// <summary>A common generic service handler.</summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public abstract class GenericServiceHandler<TService, TRequest> : IHandleMessages<TRequest>
    {
        /// <summary>Initializes a new instance of the <see cref="GenericServiceHandler{TService, TRequest}"/> class.</summary>
        protected GenericServiceHandler(TService service, IMessageContext context)
        {
            Context = context;
            Service = service;
        }

        /// <summary>Gets the service.</summary>
        protected TService Service { get; private set; }

        /// <summary>Gets the context.</summary>
        protected IMessageContext Context { get; private set; }

        /// <inheritdoc/>
        public Task Handle(TRequest message) =>
            HandleAsync(message, Context.GetCancellationToken());

        /// <summary>Handles the request asynchronous.</summary>
        protected abstract Task HandleAsync(TRequest message, CancellationToken cancellationToken);
    }
}
