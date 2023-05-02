namespace DevFreela.Core.Services.Interfaces
{
    public interface IMessageBusService
    {
        void Publish(string queue, byte[] message);
    }
}
