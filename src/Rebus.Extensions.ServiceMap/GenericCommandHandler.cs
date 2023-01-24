using System;
using System.Threading;
using System.Threading.Tasks;

using Rebus.Pipeline;

#nullable enable

namespace Rebus.Extensions.ServiceMap
{
    /// <summary>A generic command handler.</summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    public class GenericCommandHandler<TService, TRequest> : GenericServiceHandler<TService, TRequest>
    {
        /// <summary>Initializes a new instance of the <see cref="GenericCommandHandler{TService, TRequest}"/> class.</summary>
        public GenericCommandHandler(
            TService service,
            IMessageContext context,
            Func<TService, TRequest, CancellationToken, Task>? func)
            : base(service, context)
        {
            Func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>Gets the function.</summary>
        protected Func<TService, TRequest, CancellationToken, Task> Func { get; private set; }

        /// <inheritdoc/>
        protected override Task HandleAsync(TRequest message, CancellationToken cancellationToken) =>
            Func(Service, message, cancellationToken);
    }
}
