using System.Text.Json.Serialization;

namespace DevFreela.Application.InputModels
{
    public class FinishProjectInputModel
    {
        [JsonIgnore]
        public int Id { get; set; }
        public string CreditCardNumber { get; set; }
        public string Cvv { get; set; }
        public string ExpiresAt { get; set; }
        public string FullName { get; set; }
    }
}