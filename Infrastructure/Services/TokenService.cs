using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Core.Contracts;
using Core.Entites.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Infrastructure.Services
{
  public class TokenService : ITokenService
  {
    private readonly IConfiguration _config;
    private readonly SymmetricSecurityKey _key;
    public TokenService(IConfiguration config)
    {
      _config = config;
      _key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Token:Key"]));
    }

    public string CreateToken(AppUser user)
    {
      var claims = new List<Claim>
      {
        new Claim(ClaimTypes.Email, user.Email),
        new Claim(ClaimTypes.GivenName, user.DisplayName) ,
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
        new Claim(JwtRegisteredClaimNames.Aud, _config["Token:Audience"]),
        new Claim(JwtRegisteredClaimNames.Iss, _config["Token:Issuer"])

      };
      var creds = new SigningCredentials(_key, SecurityAlgorithms.HmacSha512Signature);
      var tokendescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.Now.AddDays(7),
        SigningCredentials = creds,
        Issuer = _config["Token:Issuer"],
        Audience = _config["Token:Audience"]

      };
      var tokenHandler = new JwtSecurityTokenHandler();
      var token1 = tokenHandler.CreateToken(tokendescriptor);
      var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Token:Key"]));
      var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
      var token = new JwtSecurityToken(

                 issuer: _config["Token:Issuer"],
                 audience: _config["Token:Audience"],
                 claims: claims,
                 expires: DateTime.Now.AddMonths(2),
                  signingCredentials: credentials
                 );

      return tokenHandler.WriteToken(token);
    }
  }
}