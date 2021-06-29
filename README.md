# Rebus.Extensions.ServiceMap

[![Nuget downloads](https://img.shields.io/nuget/v/rebus.extensions.servicemap.svg)](https://www.nuget.org/packages/Rebus.Extensions.ServiceMap/)
[![Nuget](https://img.shields.io/nuget/dt/rebus.extensions.servicemap)](https://www.nuget.org/packages/Rebus.Extensions.ServiceMap/)
[![Build status](https://github.com/rosenkolev/rebus-extensions-servicemap/actions/workflows/github-actions.yml/badge.svg)](https://github.com/rosenkolev/rebus-extensions-servicemap/actions/workflows/github-actions.yml)
[![codecov](https://codecov.io/gh/rosenkolev/rebus-extensions-servicemap/branch/main/graph/badge.svg?token=C1DW0GQ0ZM)](https://codecov.io/gh/rosenkolev/rebus-extensions-servicemap)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/rosenkolev/rebus-extensions-servicemap/blob/main/LICENSE)

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
