using DevFreela.Core.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace DevFreela.Infrastructure.Auth
{
    public class AuthService : IAuthService
    {
        private readonly IConfiguration _configuration;
        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(string email, string role)
        {
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            // Criar chave secreta junto a uma chave de segurança simetrica.
            // O qual vai ser utilizada junto ao algoritimo de assinatura, que vai utilizar o algoritmo de hashing (SHA256)
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            // As credenciais vao ser utilizadas para assinar o token com todas informações (Algoritmo e dados do token JWT)
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            // O Claim é uma informação sobre aquele usuario a qual o token pertence 
            var claims = new List<Claim>
            {
                new Claim("userName", email),
                new Claim(ClaimTypes.Role, role) // Validações de autorização será buscada pela role no token.
            };

            var token = new JwtSecurityToken(
                issuer: issuer, 
                audience: audience, 
                expires: DateTime.Now.AddMinutes(3), 
                signingCredentials: credentials, 
                claims: claims);

            var tokenHandler = new JwtSecurityTokenHandler();
            var stringToken = tokenHandler.WriteToken(token);
            return stringToken;
        }

        public string ComputeSha256Hash(string password)
        {
            // Criando uma instancia da implementacao SHA256
            // Vai ser capaz de obter o password e criar o hash
            using (SHA256 sha256Hash = SHA256.Create())
            {
                //ComputeHash - retorna byte array
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(password));

                //Converte byte array para string (Usado para concatenação - melhor performance)
                StringBuilder builder = new StringBuilder();

                for (int i = 0; i < bytes.Length; i++)
                {
                    // 2x faz com que seja convertido em representação hexadecimal
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
