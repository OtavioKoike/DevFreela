using DevFreela.Core.DTOs;
using DevFreela.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace DevFreela.Infrastructure.Payments
{
    public class PaymentService : IPaymentService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly string _paymentsBaseUrl;

        private readonly IMessageBusService _messageBusService;
        private const string QUEUE_NAME = "Payments";
        public PaymentService(IMessageBusService messageBusService, IHttpClientFactory httpClientFactory, IConfiguration configuration)
        {
            _httpClientFactory = httpClientFactory;
            _paymentsBaseUrl = configuration.GetSection("Services:Payments").Value;

            _messageBusService = messageBusService;
        }

        // Mudado de Task<bool> para void pois com Mensageria não é possivel mais saber se deu certo no momento
        public void ProcessPayment(PaymentInfoDTO paymentInfoDTO)
        {
            var paymentInfoJson = JsonSerializer.Serialize(paymentInfoDTO);

            // Chamada ao Microsserviço de Pagamentos SEM Mensageria
            //var url = $"{_paymentsBaseUrl}/api/payments";

            //var paymentInfoContent = new StringContent(paymentInfoJson, Encoding.UTF8, "application/json");

            //var httpClient = _httpClientFactory.CreateClient("Payments");
            //var response = await httpClient.PostAsync(url, paymentInfoContent);

            //return response.IsSuccessStatusCode;
            // ------------------------------------------------------------------

            // Chamada ao Microsserviço de Pagamentos COM Mensageria
            var paymentInfoBytes = Encoding.UTF8.GetBytes(paymentInfoJson);

            _messageBusService.Publish(QUEUE_NAME, paymentInfoBytes);
        }
    }
}
