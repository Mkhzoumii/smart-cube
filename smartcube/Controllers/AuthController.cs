using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Data.SqlClient;
using System.Data;
using System.Security.Cryptography;
using smartcube.Helpers;
using smartcube.DataContext;
using smartcube.Dto;
using System.Linq;

namespace smartcube.Controllers
{
    [Authorize]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly DataContextDapper _dapper;
        private readonly IConfiguration _config;
        private readonly AuthHelper _authHelper;

        public AuthController(IConfiguration config)
        {
            _dapper = new DataContextDapper(config);
            _config = config;
            _authHelper = new AuthHelper(config);
        }

        [AllowAnonymous]
        [HttpPost("Register")]
        public IActionResult Register(UserForRegistrationDto userForRegistration)
        {
            if (userForRegistration.Password == userForRegistration.PasswordConfirm)
            {
                string sqlCheckUserExists = "SELECT email FROM Users WHERE email = @Email";
                var existsUser = _dapper.LoadData<string>(sqlCheckUserExists, new { email = userForRegistration.Email });

                if (!existsUser.Any())
                {
                    byte[] PasswordSalt = new byte[120 / 8];
                    using (RandomNumberGenerator ran = RandomNumberGenerator.Create())
                    {
                        ran.GetNonZeroBytes(PasswordSalt);
                    }

                    byte[] PasswordHash = _authHelper.GetPasswordHash(userForRegistration.Password, PasswordSalt);

                    string sqlAddAuth = @"INSERT INTO Users 
                        ([first_name], [last_name], [email], [hash_password], [salt_password]) 
                        VALUES (@FirstName, @LastName, @Email, @PasswordHash, @PasswordSalt)";

                    List<SqlParameter> sqlParameters = new List<SqlParameter>
                    {
                        new SqlParameter("@FirstName", SqlDbType.NVarChar) { Value = userForRegistration.FirstName },
                        new SqlParameter("@LastName", SqlDbType.NVarChar) { Value = userForRegistration.LastName },
                        new SqlParameter("@Email", SqlDbType.NVarChar) { Value = userForRegistration.Email },
                        new SqlParameter("@PasswordSalt", SqlDbType.VarBinary) { Value = PasswordSalt },
                        new SqlParameter("@PasswordHash", SqlDbType.VarBinary) { Value = PasswordHash }
                    };

                    if (_dapper.ExecuteSqlWithParameters(sqlAddAuth, sqlParameters))
                    {
                        return Ok();
                    }

                    return StatusCode(500, "Failed to register user.");
                }

                return Conflict("User with this email already exists.");
            }

            return BadRequest("Passwords do not match.");
        }

        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login(UserForLoginDto userForLoginDto)
        {
            string sqlForHashAndSalt = @"SELECT [hash_password], [salt_password]
                                         FROM Users 
                                         WHERE email = @Email";

            var userForLoginConformationDto = _dapper.LoadDataSingle<UserForLoginConformationDto>(
                sqlForHashAndSalt,
                new { Email = userForLoginDto.email });

            if (userForLoginConformationDto == null)
            {
                return Unauthorized("Invalid email or password.");
            }

            byte[] passwordHash = _authHelper.GetPasswordHash(userForLoginDto.Password, userForLoginConformationDto.salt_password);

            if (!passwordHash.SequenceEqual(userForLoginConformationDto.hash_password))
            {
                return Unauthorized("Incorrect password.");
            }

            string userSql = @"SELECT [user_id], [first_name], [email]
                               FROM Users WHERE email = @Email";

            var userResult = _dapper.LoadDataSingle<InfoForCreateToken>(
                userSql,
                new { email = userForLoginDto.email }); // التعديل هنا

            if (userResult == null)
            {
                return Unauthorized("User not found.");
            }

            string token = _authHelper.CreateToken(userResult.user_id, userResult.email, userResult.first_name);

            return Ok(new
            {
                token,
                first_name = userResult.first_name,
                email = userResult.email
            });
        }
    }
}
