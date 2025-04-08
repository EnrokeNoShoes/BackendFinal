using Microsoft.IdentityModel.Tokens;
using Proyecto_Final.Shared.Referenciales;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ProyectoFinal.Custom
{
    public class Utilidades
    {
        private readonly IConfiguration _configuration;
        public Utilidades(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string encriptarSHA256(string texto)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(texto));
                StringBuilder builder = new StringBuilder();
                for (int i=0;i<bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }
                return builder.ToString();
            }
        }

        public string generarJWT(Usuario modelo)
        {

            if (modelo == null)
                 throw new ArgumentNullException(nameof(modelo), "El modelo no puede ser null.");

            // Verifica que las propiedades necesarias estén inicializadas
            if (modelo.codusu == null || modelo.nomusu == null )
                throw new InvalidOperationException("El modelo debe contener valores válidos para codusu, nomusu.");

            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, modelo.codusu.ToString()),
                new Claim(ClaimTypes.Name, modelo.nomusu.ToString()),
            };
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:key"]));
            var credential = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);
            var jwtConfig = new JwtSecurityToken
            (
                claims: userClaims,
                expires: DateTime.UtcNow.AddMinutes(60),
                signingCredentials: credential
                ) ;
            return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
        }
    }
}
