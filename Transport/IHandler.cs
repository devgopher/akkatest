namespace Transport
{
    public interface IHandler<in T>
    where T : class
    {
        public Task Handle(T receivedMessage);
    }
}
