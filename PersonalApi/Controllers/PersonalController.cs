using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PersonalApi.Modelle;
using System.ComponentModel.DataAnnotations;
using System.Security.Cryptography.X509Certificates;
using PersonalApi.Datenbank;
using Microsoft.EntityFrameworkCore;
using PersonalApi.DTOs;
using AutoMapper;



namespace PersonalApi.Controllers;


[Route("api/[controller]")]
[ApiController]
public class PersonalControlller : ControllerBase
{
    private readonly PersonalKontext datenbankKontext;
    private readonly IMapper mapper;

    public PersonalControlller(PersonalKontext kontext, IMapper mapper)
    {
        datenbankKontext = kontext;
        this.mapper = mapper;
    }
     
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Mitarbeiter>>> AlleMitarbeiterAbrufenAsync()
    {
        var mitarbeiterListe = await datenbankKontext.Mitarbeiter.ToListAsync();
        var antwortListe = mapper.Map<List<MitarbeiterAntwortDto>>(mitarbeiterListe);     
        return Ok(antwortListe);
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<Mitarbeiter>> EinMitarbeiterAbrufen(int mitarbeiterId)
    {
        var gesuchterMitarbeiter = await datenbankKontext.Mitarbeiter.FirstOrDefaultAsync(m => m.MitarbeiterId == mitarbeiterId);

        if (gesuchterMitarbeiter != null)
        {
            return NotFound($"Fehler: Der Mitarbeiter mit der ID {mitarbeiterId} existiert nicht");
        }

        return Ok(gesuchterMitarbeiter);
    }
    
    // Erstellen
    [HttpPost]
    public async Task<ActionResult<MitarbeiterErstellenDto>> MitarbeiterAnlegenAsync([FromBody] MitarbeiterErstellenDto neuerArbeiter)
    {
        var mitarbeiter = mapper.Map<Mitarbeiter>(neuerArbeiter);
        await datenbankKontext.Mitarbeiter.AddAsync(mitarbeiter);
        await datenbankKontext.SaveChangesAsync();

        var antwort = mapper.Map<MitarbeiterAntwortDto>(mitarbeiter);

        return CreatedAtAction(
            nameof(EinMitarbeiterAbrufen), 
            new { id = antwort.MitarbeiterId }, 
            antwort
        );
    }
    // Aktualisieren
    [HttpPut("{id}")]
    public async Task<IActionResult> MitarbeiterAktualisierenAsync(int id, [FromBody] MitarbeiterAktualisierenDto neueDaten)
    {
        if (id != neueDaten.MitarbeiterId)
        {
            return BadRequest("Die ID in der URL stimmt nicht mit der ID im Datensatz überein.");
        }
        var gesuchterMitarbeiter = await datenbankKontext.Mitarbeiter.FirstOrDefaultAsync(m => m.MitarbeiterId == id);
        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {id} nicht gefunden.");
        }
        mapper.Map(gesuchterMitarbeiter, neueDaten);
        await datenbankKontext.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> MitarbeiterLoeschenAsync(int id)
    {
        var gesuchterMiarbeiter = await datenbankKontext.Mitarbeiter.FirstOrDefaultAsync(m => m.MitarbeiterId == id);
        if(gesuchterMiarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {id} existiert nicht.");
        }
        datenbankKontext.Mitarbeiter.Remove(gesuchterMiarbeiter);
        await datenbankKontext.SaveChangesAsync();

        return NoContent();
    }
}
