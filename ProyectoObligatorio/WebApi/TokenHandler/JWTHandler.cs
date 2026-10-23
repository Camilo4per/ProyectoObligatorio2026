using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Dominio.Entidades;
using DTOs.DTOs;

namespace ApiWeb.TokenHandler
{
    public class JWTHandler
    {
        public static string GenerarToken(UsuarioDTO usuario, IConfiguration configuration)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var clave = Encoding.ASCII.GetBytes(configuration["Jwt:Key"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, usuario.NombreUsuario),
                    new Claim(ClaimTypes.Email, usuario.Email),
                    new Claim(ClaimTypes.Role, ((RolesUsuario)usuario.Rol).ToString())
                }),
                Expires = DateTime.UtcNow.AddMonths(1),
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(clave), 
                    SecurityAlgorithms.HmacSha256Signature
                )
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}