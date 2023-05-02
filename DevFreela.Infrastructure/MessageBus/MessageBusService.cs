using DevFreela.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

namespace DevFreela.Infrastructure.MessageBus
{
    public class MessageBusService : IMessageBusService
    {
        private readonly ConnectionFactory _factory;

        // Em caso de servidor RabbitMQ externo, é preciso adicionar informações de configuração de usuario e senha
        public MessageBusService(IConfiguration configuration)
        {
            _factory = new ConnectionFactory
            {
                HostName = "localhost"
            };
        }

        public void Publish(string queue, byte[] message)
        {
            // Inicializar a conexao com RabbitMQ
            using (var connection = _factory.CreateConnection())
            {
                // Criar um canal de comunicacao para realizar operações de fila, publicar mensagens...
                using (var channel = connection.CreateModel())
                {
                    // Garantir que a fila esteja criada // Se nao tiver criada vai criar nesse momento
                    channel.QueueDeclare(queue: queue, durable: false, exclusive: false, autoDelete: false, arguments: null);

                    // Definir como vai publicar a mensagem
                    channel.BasicPublish(exchange: "", routingKey: queue, basicProperties: null, body: message);

                    // Publicar a mensagem

                }
            }
        }
    }
}
