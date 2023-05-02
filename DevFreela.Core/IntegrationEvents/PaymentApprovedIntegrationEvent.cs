namespace DevFreela.Core.IntegrationEvents
{
    public class PaymentApprovedIntegrationEvent
    {
        public PaymentApprovedIntegrationEvent(int idProject)
        {
            IdProject = idProject;
        }

        public int IdProject { get; set; }
    }
}
