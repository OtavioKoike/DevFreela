using DevFreela.Core.DTOs;

namespace DevFreela.Core.Services.Interfaces
{
    public interface IPaymentService
    {
        // Mudado de Task<bool> para void pois com Mensageria não é possivel mais saber se deu certo no momento
        void ProcessPayment(PaymentInfoDTO paymentInfoDTO);
    }
}
