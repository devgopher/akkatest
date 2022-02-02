namespace Transport;

public interface IProducer<TMsg>
    where TMsg : class
{
    public Task<bool> ProduceAsync(TMsg message);
}