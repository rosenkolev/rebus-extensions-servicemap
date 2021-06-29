using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;

using NSubstitute;

using Rebus.Bus;
using Rebus.Handlers;
using Rebus.Messages;
using Rebus.Pipeline;
using Rebus.Transport;

namespace Rebus.Extensions.ServiceMap.Tests
{
    [TestClass]
    public class RebusExtensionsTests
    {
        [TestMethod]
        public async Task AddRebusCommandUsesGenericHandler()
        {
            var services = new ServiceCollection();
            services.AddSingleton<Service>();
            services.AddSingleton(GetMessageContext());
            services.AddRebusCommand<Service, object>(
                (service, req, cancellationToken) => service.Command(req, cancellationToken));

            using var provider = services.BuildServiceProvider();
            var handler = provider.GetService<IHandleMessages<object>>();
            var message = new object();

            await handler.Handle(message);

            var service = provider.GetService<Service>();
            Assert.AreSame(message, service.Data);
        }

        [TestMethod]
        public async Task AddRebusRequestUsesGenericHandler()
        {
            var bus = Substitute.For<IBus>();
            var services = new ServiceCollection();            
            services.AddSingleton(bus);
            services.AddSingleton(GetMessageContext());
            services.AddSingleton<Service>();
            services.AddRebusRequest<Service, object, string>(
                (service, req, cancellationToken) => service.Request(req, cancellationToken));

            using var provider = services.BuildServiceProvider();
            var handler = provider.GetService<IHandleMessages<object>>();
            var message = new object();

            await handler.Handle(message);

            var service = provider.GetService<Service>();
            Assert.AreSame(message, service.Data);
            await bus.Received().Reply("123", null);
        }

        private static IMessageContext GetMessageContext()
        {
            var context = Substitute.For<IMessageContext>();
            var headers = new Dictionary<string, string>();
            var body = Array.Empty<byte>();
            var scope = new RebusTransactionScope();
            context.IncomingStepContext.Returns(
                new IncomingStepContext(new TransportMessage(headers, body), scope.TransactionContext));
            
            return context;
        }

        private class Service
        {
            public Task Command(object request, CancellationToken cancellationToken)
            {
                Data = request;
                return Task.CompletedTask;
            }

            public Task<string> Request(object request, CancellationToken cancellationToken)
            {
                Data = request;
                return Task.FromResult("123");
            }

            public object Data { get; private set; }
        }
    }
}
