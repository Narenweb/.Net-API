using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TaskAPI.Model;

namespace TaskAPI.Data
{
    public class AuthRepository : IAuthRepository
    {
        private readonly GetDbContext _Context;
        private readonly IConfiguration _configuration;

        public AuthRepository(GetDbContext Context, IConfiguration configuration)
        {
            _configuration = configuration;
            _Context = Context;

        }

        async Task<ServiceResponse<string>> IAuthRepository.Login(string username, string password)
        {
            var response = new ServiceResponse<string>();
            try
            {
                var user = _Context.Users.FirstOrDefault(u => u.Username.ToLower().Equals(username.ToLower()));
                if (user is null)
                {
                    throw new Exception("User Not Found");

                }
                else if (!VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                {
                    throw new Exception("Wrong Password");
                }
                response.Success = true;
                response.Message = "found";
                response.Data = CreateToken(user);

            }
            catch(Exception e)
           
            {
                response.Success = false;
                response.Message = e.Message;
                response.Data = null;
            }

            return response;
        }

        async Task<ServiceResponse<int>> IAuthRepository.Register(Users user, string password)
        {
            var response = new ServiceResponse<int>();
            try
            {
                bool UserExists = (await _Context.Users.AnyAsync(u => u.Username.ToLower() == user.Username.ToLower()));
                if (UserExists)
                {
                    throw new Exception("User already Exists");
                }
                CreatePasswordHash(password, out byte[] passwordHash, out byte[] passwordSalt);
                user.PasswordHash = passwordHash;
                user.PasswordSalt = passwordSalt;

                _Context.Users.Add(user);
                await _Context.SaveChangesAsync();
                response.Data = user.Id.ToString();
                response.Success = true;
                response.Message = "Success";
                return response;

            }
            catch (Exception e)
            {
                response.Success = false;
                response.Message = e.Message;

            }
            return response;
        }


            private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));

            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);


            }
        }

        private string CreateToken(Users user)
        {
            var Claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,user.Username),
            };
            var appSettingsToken = _configuration.GetSection("AppSettings:Token").Value;
            if (appSettingsToken is null)
            {
                throw new Exception("AppSettings Token is null!");

            }
            SymmetricSecurityKey key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(appSettingsToken));
            SigningCredentials creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var tokenDiscriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(Claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDiscriptor);
            return tokenHandler.WriteToken(token);

        }

      
    }
}

           
