using DevFreela.Core.IntegrationEvents;
using DevFreela.Core.Repositories;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace DevFreela.Infrastructure.Consumers
{
    // Ficara rodando, esperando a mensagem e disparando evento interno para processa-la
    public class PaymentApprovedConsumer : BackgroundService
    {
        private const string PAYMENT_APPROVED_QUEUE = "PaymentsApproved";
        private readonly IConnection _connection;
        private readonly IModel _channel;
        // Necessario quando for realizar acesso a um serviço injetado com ciclo de vida como scoped
        // Pois esse serviço roda indefinidamente, sepreciso utilizar um serviço scoped, preciso cria-lo internamente
        private readonly IServiceProvider _serviceProvider;
        public PaymentApprovedConsumer(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;

            var factory = new ConnectionFactory
            {
                HostName = "localhost"
            };

            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            // Fila de pagamentos Aprovados
            _channel.QueueDeclare(queue: PAYMENT_APPROVED_QUEUE, durable: false, exclusive: false, autoDelete: false, arguments: null);

        }

        // Metodo que ficará escutando a fila
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var consumer = new EventingBasicConsumer(_channel);

            // Evento caso receba mensage // Como a mensagem vai ser tratada
            // Received -> Acessar o novo evento
            // eventArgs -> Acessar informações da mensagem
            consumer.Received += async (sender, eventArgs) =>
            {
                var byteArray = eventArgs.Body.ToArray();
                var paymentApprovedJson = Encoding.UTF8.GetString(byteArray);
                var paymentApproved = JsonSerializer.Deserialize<PaymentApprovedIntegrationEvent>(paymentApprovedJson);

                await FinishProject(paymentApproved.IdProject);

                // Avisar ao Message Broker que a mensagem foi recebida
                _channel.BasicAck(eventArgs.DeliveryTag, false);
            };

            // Inicializar o consumo de mensagem
            _channel.BasicConsume(PAYMENT_APPROVED_QUEUE, false, consumer);
            return Task.CompletedTask;
        }

        private async Task FinishProject(int id)
        {
            // Criar um scope para criar instancias
            using (var scope = _serviceProvider.CreateScope())
            {
                // Antes recebia IPaymentService por Injeção de Dependencia, porem agora não é possivel por ser um metodo que fica rodando indefinidamente
                var projectRepository = scope.ServiceProvider.GetRequiredService<IProjectRepository>();
                
                var project = await projectRepository.GetByIdAsync(id);
                project.Finish();

                await projectRepository.SaveChangesAsync();
            }
        }
    }
}
