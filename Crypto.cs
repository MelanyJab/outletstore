using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Web;
using System.Web.Http.Controllers;

namespace outletstore.Functions
{
    public class Crypto
    {
        private const string secretKey = "3D46EAEDC8F285318F7971A46A3D87A3A27635AC378DD151A3B41E57019B2680";
        private static readonly SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));

        public string GenerateToken(string id, string email)
        {
            var claims = new[] {
                new Claim("userId", id),
                new Claim("Email", email)
            };
            var token = new JwtSecurityToken(
                claims: claims,
                signingCredentials: new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256)
            );
            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }

        public bool IsTokenValid(string token, ref HttpActionContext actionContext)
        {
            SecurityToken validatedToken;
            ClaimsPrincipal claimsPrincipal;
            if (string.IsNullOrEmpty(token))
            {
                return false;
            }

            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtToken = tokenHandler.ReadJwtToken(token);
                TokenValidationParameters tokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = securityKey,
                    RequireExpirationTime = false
                };
                claimsPrincipal = tokenHandler.ValidateToken(token, tokenValidationParameters, out validatedToken);
            }
            catch (SecurityTokenSignatureKeyNotFoundException)
            {
                return false;
            }

            int userId = int.Parse(claimsPrincipal.FindFirst("userId").Value);
            actionContext.Request.Properties["userId"] = userId;
            return true;
        }
    }
}


