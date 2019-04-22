using Microsoft.IdentityModel.Tokens;
using netCoreMvcTest.Data;
using netCoreMvcTest.Ioc;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace netCoreMvcTest
{
    public static class JwtTokenExtensionMethods
    {
        /// <summary>
        /// generate jwt bearer token
        /// </summary>
        ///<param name="user">user details</param>
        /// <returns></returns>
        public static string GenerateJwtToken(this ApplicationUser user)
        {

            //set token claims
            //https://tools.ietf.org/html/rfc7519#section-4.1 - about jwt
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString("N")),
                new Claim(JwtRegisteredClaimNames.NameId, user.UserName),
                new Claim("my key", "my value")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IocContainer.Configuration["Jwt:SecretKey"]));

            //create credentials used to generate tokens
            var credentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(IocContainer.Configuration["Jwt:SecretKey"])),
                SecurityAlgorithms.HmacSha256);

            //generrate jwt token 

            var token = new JwtSecurityToken(
                issuer: IocContainer.Configuration["Jwt:Issuer"],
                audience: IocContainer.Configuration["Jwt:Audience"],
                claims: claims,
                signingCredentials: credentials,
                expires: DateTime.Now.AddMonths(3));

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
