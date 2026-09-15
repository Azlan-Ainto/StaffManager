using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using PersonalApi.DTOs;
using PersonalApi.Modelle;
using PersonalApi.Repositories;

namespace PersonalApi.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PersonalController : ControllerBase
{
    private readonly IPersonalRepository personalRepository;
    private readonly IMapper mapper;

    public PersonalController(IPersonalRepository repository,IMapper mapper)
    {
        personalRepository = repository;
        this.mapper = mapper;
    }

 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MitarbeiterAntwortDto>>> Alle_Mitarbeiter_Abrufen_Async()
    {
        var mitarbeiterListe =  await personalRepository.Hole_Alle_Mitarbeiter_Async();

        var antwortListe =  mapper.Map<List<MitarbeiterAntwortDto>>(mitarbeiterListe);

        return Ok(antwortListe);
    }


    [HttpGet("{arbeiterId}")]
    public async Task<ActionResult<MitarbeiterAntwortDto>> Mitarbeiter_Abrufen_Async(int arbeiterId)
    {

        var gesuchterMitarbeiter =  await personalRepository.Hole_Mitarbeiter_Nach_Id_Async(arbeiterId);

        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {arbeiterId} existiert nicht.");
        }

        var antwort = mapper.Map<MitarbeiterAntwortDto>(gesuchterMitarbeiter);

        return Ok(antwort);
    }

 
    [HttpPost]
    public async Task<ActionResult<MitarbeiterAntwortDto>> Mitarbeiter_Anlegen_Async([FromBody] MitarbeiterErstellenDto neuerArbeiter)
    {

        var mitarbeiter = mapper.Map<Mitarbeiter>(neuerArbeiter);

        await personalRepository.Mitarbeiter_Hinzufuegen_Async(mitarbeiter);

        await personalRepository.Aktualisieren_Async();

        var antwort =  mapper.Map<MitarbeiterAntwortDto>(mitarbeiter);

        // Location-Header zeigt auf GET api/Personal/{arbeiterId} des neu angelegten Mitarbeiters
        return CreatedAtAction(nameof(Mitarbeiter_Abrufen_Async), new { arbeiterId = antwort.MitarbeiterId }, antwort);
    }


    [HttpPut("{arbeiterId}")]
    public async Task<IActionResult> Mitarbeiter_Aktualisieren_Async(int arbeiterId, [FromBody] MitarbeiterAktualisierenDto neueDaten)
    {

        if (arbeiterId != neueDaten.MitarbeiterId)
        {
            return BadRequest("Die ID in der URL stimmt nicht mit der ID im Datensatz überein.");
        }

        var gesuchterMitarbeiter =  await personalRepository.Hole_Mitarbeiter_Nach_Id_Async(arbeiterId);


        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {arbeiterId} nicht gefunden.");
        }

        mapper.Map(neueDaten, gesuchterMitarbeiter);

        await personalRepository.Aktualisieren_Async();

        return NoContent();
    }


    [HttpDelete("{arbeiterId}")]
    public async Task<IActionResult>  Mitarbeiter_Loeschen_Async(int arbeiterId)
    {

        var gesuchterMitarbeiter = await personalRepository.Hole_Mitarbeiter_Nach_Id_Async(arbeiterId);

        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {arbeiterId} existiert nicht.");
        }

        await personalRepository.Mitarbeiter_Loeschen_Async(gesuchterMitarbeiter);

        await personalRepository.Aktualisieren_Async();

        return NoContent();
    }
}