using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using APIPoliza.Models;
using System.Text;


[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IConfiguration _config;

    // Inyectamos IConfiguration para leer el appsettings.json
    public AuthController(IConfiguration config)
    {_config = config;}

    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest login)
    {
        // --- Validación de Usuario ---
        if (login.UserPoliza == "admin" && login.PasswordPoliza == "12345")
        {
            // Si el usuario es válido, generamos el token
            var tokenString = GenerateJwtToken(login.UserPoliza);
            return Ok(new { token = tokenString });
        }
        return Unauthorized("Usuario o contraseña incorrectos.");
    }

    private string GenerateJwtToken(string username)
    {
        // Leemos la configuración del appsettings.json
        var jwtKey = _config["Jwt:Key"];
        var jwtIssuer = _config["Jwt:Issuer"];
        var jwtAudience = _config["Jwt:Audience"];

        if (string.IsNullOrEmpty(jwtKey))
        {
            throw new InvalidOperationException("La llave JWT (Jwt:Key) no está configurada.");
        }

        // 1. Creamos la llave de seguridad
        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 2. Creamos los "Claims" (información que va dentro del token)
        // Puedes agregar más claims, como el ID del usuario, roles, etc.
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, username), // El "subject", usualmente el username o ID
            new Claim(JwtRegisteredClaimNames.Email, $"{username}@miapi.com"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()) // Un ID único para el token
        };

        // 3. Creamos el Token
        var token = new JwtSecurityToken(
            issuer: jwtIssuer,
            audience: jwtAudience,
            claims: claims,
            expires: DateTime.Now.AddMinutes(120), // Tiempo de expiración (ej: 2 horas)
            signingCredentials: credentials);

        // 4. Lo escribimos como un string
        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}