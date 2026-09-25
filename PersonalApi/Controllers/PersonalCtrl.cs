using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PersonalApi.DTOs;
using PersonalApi.Geschaeftslogik;
using PersonalApi.Modelle;
using PersonalApi.Repositories;

namespace PersonalApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class PersonalCtrl : ControllerBase
{
    private readonly IPersonalRepository _personalRepository;
    private readonly IPersonalService _personalService;
    private readonly IMapper _mapper;

    public PersonalCtrl(IPersonalRepository repository,IPersonalService personalService, IMapper mapper)
    {

        _personalRepository = repository;

        _mapper = mapper;

        _personalService = personalService;
    }

 
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MitarbeiterAntwortDto>>> Alle_Mitarbeiter_Abrufen_Async()
    {

        var mitarbeiterListe =  await _personalRepository.Hole_Alle_Mitarbeiter_Async();

        var antwortListe =  _mapper.Map<List<MitarbeiterAntwortDto>>(mitarbeiterListe);

        return Ok(antwortListe);
    }


    [HttpGet("{arbeiterId}")]
    public async Task<ActionResult<MitarbeiterAntwortDto>> Mitarbeiter_Abrufen_Async(int arbeiterId)
    {

        var gesuchterMitarbeiter =  await _personalRepository.Hole_Mitarbeiter_Nach_Id_Async(arbeiterId);

        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {arbeiterId} existiert nicht.");
        }

        var antwort = _mapper.Map<MitarbeiterAntwortDto>(gesuchterMitarbeiter);

        return Ok(antwort);
    }

 
    [HttpPost]
    public async Task<ActionResult<MitarbeiterAntwortDto>> Mitarbeiter_Anlegen_Async([FromBody] MitarbeiterErstellenDto neuerArbeiter)
    {
        try
        {
            var antwort = await _personalService.Mitarbeiter_Anlegen_Async(neuerArbeiter);

            return CreatedAtAction(nameof(Mitarbeiter_Abrufen_Async), new { arbeiterId = antwort.MitarbeiterId }, antwort);

        }
        catch (ArgumentException fehler)
        {
            return BadRequest(fehler.Message);

        }
    }


    [HttpPut("{arbeiterId}")]
    public async Task<IActionResult> Mitarbeiter_Aktualisieren_Async(int arbeiterId, [FromBody] MitarbeiterAktualisierenDto neueDaten)
    {

        if (arbeiterId != neueDaten.MitarbeiterId)
        {
            return BadRequest("Die ID in der URL stimmt nicht mit der ID im Datensatz überein.");
        }

        var gesuchterMitarbeiter =  await _personalRepository.Hole_Mitarbeiter_Nach_Id_Async(arbeiterId);


        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {arbeiterId} nicht gefunden.");
        }

        _mapper.Map(neueDaten, gesuchterMitarbeiter);

        await _personalRepository.Aktualisieren_Async();

        return NoContent();
    }


    [HttpDelete("{arbeiterId}")]
    public async Task<IActionResult>  Mitarbeiter_Loeschen_Async(int arbeiterId)
    {

        var gesuchterMitarbeiter = await _personalRepository.Hole_Mitarbeiter_Nach_Id_Async(arbeiterId);

        if (gesuchterMitarbeiter == null)
        {
            return NotFound($"Mitarbeiter mit ID {arbeiterId} existiert nicht.");
        }

        await _personalRepository.Mitarbeiter_Loeschen_Async(gesuchterMitarbeiter);

        await _personalRepository.Aktualisieren_Async();

        return NoContent();
    }



    // Den [HttpGet("suche")] Endpunkt von gestern hiermit ersetzen:
    [HttpGet("suche")]
    public async Task<ActionResult<PaginierteAntwortDto<MitarbeiterAntwortDto>>> Mitarbeiter_Suchen_Async([FromQuery] MitarbeiterSuchParameterDto suchParameter)
    {

        var ergebnisse = await _personalService.Suche_Und_Paginiere_Mitarbeiter_Async(suchParameter);

        return Ok(ergebnisse);
    }
}