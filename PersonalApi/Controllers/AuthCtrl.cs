using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;

using PersonalApi.DTOs;

namespace PersonalApi.Controllers;

public class AuthCtrl: ControllerBase
{
    public readonly IConfiguration _configuration;

    public AuthCtrl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("anmelden")]
    public IActionResult Anmelden([FromBody] AnmeldeDatenDto anmeldeDaten)
    {
     
        if(anmeldeDaten.Benutzername == "Admin" && anmeldeDaten.Password == "Geheim2026")
        {
            var generierterToken = GeneriereJwtToken(anmeldeDaten.Benutzername);
            return Ok(new { Token = generierterToken });
        }
        return Unauthorized("Ungültiger Benutzername oder Passwort.");
    }

    private object GeneriereJwtToken(string benutzername)
    {
        var SchluesselText = _configuration["JwTEinstellungen:Sicherheitsschluessel"];
        var sicherheitsschluessel = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SchluesselText));
        var anmeldeinformation =  new SigningCredentials(sicherheitsschluessel, SecurityAlgorithms.HmacSha256);
        // Informationen(Claims), die in Token gespeichert werden sollen
        var eigenschaften = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, benutzername),
            new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())
        };

        var token = new JwtSecurityToken(
             issuer:            _configuration["JwtEinstellungen:Aussteller"],
             audience:          _configuration["JwtEinstellungen:Zuschauer"],
             expires:           DateTime.Now.AddHours(2),
             signingCredentials: anmeldeinformation
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
