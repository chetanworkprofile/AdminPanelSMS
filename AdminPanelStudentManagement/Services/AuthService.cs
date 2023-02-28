using AdminPanelStudentManagement.Data;
using AdminPanelStudentManagement.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AdminPanelStudentManagement.Services
{
    public class AuthService:IAuthService
    {
        Response response = new Response();
        private readonly AdminSMSAPIDbContext DbContext;
        private readonly IConfiguration _configuration;
        public AuthService(AdminSMSAPIDbContext dbContext, IConfiguration configuration)
        {
            this.DbContext = dbContext;
            this._configuration = configuration;
        }

        public Response AdminLogin(UserDTO request)
        {
            var admin = DbContext.Admin.Find(1);
            if(admin != null && request.Password == admin.Password)
            {
                response.StatusCode = 200;
                response.Message= "Login Successful";
                User user = new User
                {
                    Username = admin.Username,
                    UserRole = "Admin"
                };
                string token = CreateToken(user);
                response.Data = token;
                return response;
            }
            response.StatusCode = 403;
            response.Message = "Wrong password.";
            response.Data = string.Empty;
            return response;
        }

        internal string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.UserRole)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                    claims: claims,
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: creds
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        public byte[] CreatePasswordHash(string password)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Password_Salt").Value!);
            byte[] passwordHash;
            using (var hmac = new HMACSHA512(salt))
            {
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
            return passwordHash;
        }
        private bool VerifyPasswordHash(string password, byte[] passwordHash)
        {
            byte[] salt = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Password_Salt").Value!);
            using (var hmac = new HMACSHA512(salt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }
    }
}
