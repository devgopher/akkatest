namespace Transport;

public interface IConsumer<TMsg>
    where TMsg : class
{
    public void Subscribe<THndl>(THndl? handler, CancellationToken token)
        where THndl : class, IHandler<TMsg>;
}