using Microsoft.Extensions.DependencyInjection;
using Transport.Kafka;

namespace Transport.Extensions;

public static class ServiceExtensions
{
    public static IServiceCollection AddKafkaProducer<TMsg>(this IServiceCollection services)
        where TMsg : class =>
        services.AddSingleton<IProducer<TMsg>, KafkaProducer<TMsg>>();


    public static IServiceCollection AddKafkaConsumer<TMsg, THndl>(this IServiceCollection services, THndl handler)
        where TMsg : class
        where THndl : class, IHandler<TMsg>
    {
        services.AddSingleton<IConsumer<TMsg>, KafkaConsumer<TMsg>>();

        var service = services.BuildServiceProvider().GetService<IConsumer<TMsg>>();
        service.Subscribe(handler, CancellationToken.None);

        return services;
    }
}