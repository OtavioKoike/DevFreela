using DevFreela.Core.DTOs;

namespace DevFreela.Core.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<bool> ProcessPayment(PaymentInfoDTO paymentInfoDTO);
    }
}
