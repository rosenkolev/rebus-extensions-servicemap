using System;
using System.Threading;
using System.Threading.Tasks;

using Rebus.Bus;
using Rebus.Pipeline;

#nullable enable

namespace Rebus.Extensions.ServiceMap
{
    /// <summary>A generic request/reply handler.</summary>
    /// <typeparam name="TService">The type of the service.</typeparam>
    /// <typeparam name="TRequest">The type of the request.</typeparam>
    /// <typeparam name="TResult">The type of the reply result.</typeparam>
    public class GenericRequestHandler<TService, TRequest, TResult> : GenericServiceHandler<TService, TRequest>
    {
        /// <summary>Initializes a new instance of the <see cref="GenericRequestHandler{TService, TRequest, TResult}"/> class.</summary>
        public GenericRequestHandler(
            TService service,
            IMessageContext context,
            IBus bus,
            Func<TService, TRequest, CancellationToken, Task<TResult>>? func)
            : base(service, context)
        {
            Bus = bus ?? throw new ArgumentNullException(nameof(bus));
            Func = func ?? throw new ArgumentNullException(nameof(func));
        }

        /// <summary>Gets the bus.</summary>
        protected IBus Bus { get; private set; }

        /// <summary>Gets the function.</summary>
        protected Func<TService, TRequest, CancellationToken, Task<TResult>> Func { get; private set; }

        /// <inheritdoc/>
        protected override async Task HandleAsync(TRequest message, CancellationToken cancellationToken)
        {
            var result = await Func(Service, message, cancellationToken);
            await Bus.Reply(result);
        }
    }
}
