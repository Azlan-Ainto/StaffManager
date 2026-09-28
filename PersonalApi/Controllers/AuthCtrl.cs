using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System;
using System.Text;

using PersonalApi.DTOs;

namespace PersonalApi.Controllers;

[Route("api/Authentifizierungs")]
[ApiController]
public class AuthCtrl: ControllerBase
{
    public readonly IConfiguration _configuration;

    public AuthCtrl(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [HttpPost("anmelden")]
    public IActionResult Anmelden(
            [FromBody] 
        AnmeldeDatenDto anmeldeDaten
    ){
        string benutzerRolle = string.Empty;
     
        if(
            anmeldeDaten.Benutzername == "Admin" && 
            anmeldeDaten.Password == "Geheim2026"
         )
        {
            benutzerRolle = "Admin";
        }
        else if(
            anmeldeDaten.Benutzername == "leser" && 
            anmeldeDaten.Password == "Lesen2026!"
        )
        {
            benutzerRolle = "Benutzer";
        }
        else
        {
            return 
                Unauthorized("Ungültiger Benutzername oder Passwort.");
        }
        var generierterToken = 
                GeneriereJwtToken(
                    anmeldeDaten.Benutzername, 
                    benutzerRolle
                 );
        return Ok(
                new {
                    Token = generierterToken, 
                    Rolle = benutzerRolle
                }
        );

    }

    private object GeneriereJwtToken(
        string benutzername, 
        string rolle
    )
    {
        // 1. Schritt Sicherheitssschlüssel
        var SchluesselText = 
            _configuration["JwTEinstellungen:Sicherheitsschluessel"];

        // 2. Bytes erzeugen
        var sicherheitsschluessel = 
            new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(SchluesselText)
             );

        // 3. Verschlüsselung der Sicherheitsschlüssel
        var anmeldeinformation =  
                new SigningCredentials(
                    sicherheitsschluessel, 
                    SecurityAlgorithms.HmacSha256
                );

       // 4.Claims erstellen
        // Informationen(Claims), die in Token gespeichert werden sollen
        var eigenschaften = new[]
        {
            new Claim(
                JwtRegisteredClaimNames.Sub, 
                benutzername
            ),
            new Claim(
                JwtRegisteredClaimNames.Jti,
                Guid.NewGuid().ToString()
            ),
            new Claim(ClaimTypes.Role, rolle)
        };
        // 5.Token erstellen
        var token = new JwtSecurityToken(
             issuer:            
                _configuration["JwtEinstellungen:Aussteller"],
             audience:          
                _configuration["JwtEinstellungen:Zuschauer"],
             claims: eigenschaften,
             expires:           
                DateTime.Now.AddHours(2),
             signingCredentials: 
                anmeldeinformation
        );

        return new JwtSecurityTokenHandler().WriteToken(token);

    }
}
