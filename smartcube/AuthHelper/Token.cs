using Microsoft.EntityFrameworkCore;
using System.IdentityModel.Tokens.Jwt;
using Dapper;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

using System.Text;

public class TokenHelper
{
    public static (string? user_id, string? email, string? first_name) GetUserDetailsFromToken(string token, string secret)
    {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = System.Text.Encoding.ASCII.GetBytes(secret);

        try
        {
            // Token validation parameters
            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false,
            };

            // Validate the token and extract the claims principal
            var principal = tokenHandler.ValidateToken(token, validationParameters, out SecurityToken validatedToken);

            // Extract the UserId claim
            var userIdClaim = principal.FindFirst("user_id");
            var user_id = userIdClaim?.Value;


            // Extract the Email claim
            var emailClaim = principal.FindFirst("email");
            var email = emailClaim?.Value;

            // Extract the FirstName claim
            var firstNameClaim = principal.FindFirst("first_name");
            var firstName = firstNameClaim?.Value;

            // Return all extracted details
            return (user_id,  email, firstName);
        }
        catch (Exception ex)
        {
            // Handle token validation failure
            Console.WriteLine("Token validation failed: " + ex.Message);
            return ( null, null, null); // Return null values if token is invalid
        }
    }
}