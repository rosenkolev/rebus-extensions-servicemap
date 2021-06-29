# Rebus.Extensions.ServiceMap

**This are Rebus extensions for short Func map to a service class.**

You can install [Rebus.Extensions.ServiceMap with NuGet](https://www.nuget.org/packages/Rebus.Extensions.ServiceMap/):

```shell
dotnet add package Rebus.Extensions.ServiceMap
```

## Map
```csharp
// startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddRebus();
    services.AddTransient<IOrdersService, OrdersService>();

    // add service map
    services
      .AddRebusCommand<IOrdersService, CreateOrderCommand>(
        (orderService, message, cancellationToken) => orderService.CreateOrderAsync(message, cancellationToken))
      .AddRebusRequest<IOrdersService, UpdateOrderRequest, UpdateOrderResponse>
        (orderService, message, cancellationToken) => orderService.UpdateOrderAsync(message, cancellationToken));
}
```

The IOrdersService should be similar to:
```csharp
interface IOrdersService
{
    Task CreateOrderAsync(CreateOrderCommand message, CancellationToken cancellationToken);
    Task<UpdateOrderResponse> UpdateOrderAsync(UpdateOrderRequest message, CancellationToken cancellationToken);
}
```

## Router default queue

```csharp
/ startup.cs
public void ConfigureServices(IServiceCollection services)
{
    services.AddRebus(
      configure => configure
        .AddDefaultRouting("main-queue")
        ...
}

```
