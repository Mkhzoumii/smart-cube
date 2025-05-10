using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace smartcube.Helpers
{
    public class AuthHelper
    {
        private readonly IConfiguration _config;

        public AuthHelper(IConfiguration config)
        {
            _config = config;
        }

        public byte[] GetPasswordHash(string password, byte[] PasswordSalt)
        {
            string? configSalt = _config.GetSection("AppSittings:PasseordKey").Value;
            if (string.IsNullOrEmpty(configSalt))
                throw new Exception("AppSettings:PasswordKey is missing in configuration.");

            string PasswordSaltPlusString = configSalt + Convert.ToBase64String(PasswordSalt);
            return KeyDerivation.Pbkdf2(
                password: password,
                salt: Encoding.ASCII.GetBytes(PasswordSaltPlusString),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8);
        }

        public string CreateToken(int user_id, string email, string first_name)
        {
            Claim[] claims = new Claim[]
            {
                new Claim("user_id", user_id.ToString()),
                new Claim("email", email),
                new Claim("first_name", first_name)
            };

            string? tokenKeyValue = _config.GetSection("AppSittings:TokenKey").Value;
            if (string.IsNullOrEmpty(tokenKeyValue))
                throw new Exception("AppSettings:TokenKey is missing in configuration.");

            SymmetricSecurityKey tokenKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(tokenKeyValue));

            SigningCredentials credentials = new SigningCredentials(tokenKey, SecurityAlgorithms.HmacSha512Signature);

            SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(2),
                SigningCredentials = credentials
            };

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}
