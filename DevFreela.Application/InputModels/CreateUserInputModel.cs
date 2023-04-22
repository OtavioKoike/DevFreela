namespace DevFreela.Application.InputModels
{
    public class CreateUserInputModel
    {
        public string FullName { get; set; }
        public string Email { get; set; }
        public DateTime birthDate { get; set; }
        public string Password { get; set; }
        public string Role { get; set; }
    }
}