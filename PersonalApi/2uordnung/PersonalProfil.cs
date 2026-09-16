using AutoMapper;
using PersonalApi.Modelle;
using PersonalApi.DTOs;

namespace PersonalApi.Zuordnung;

public class PersonalProfil : Profile
{
    public PersonalProfil()
    {
        // POST: Die Id vergibt SQL Server, das Einstellungsdatum setzt die Klasse Mitarbeiter selbst.
        CreateMap<MitarbeiterErstellenDto, Mitarbeiter>()
            .ForMember(ziel => ziel.MitarbeiterId, optionen => optionen.Ignore())
            .ForMember(ziel => ziel.Einstellungsdatum, optionen => optionen.Ignore());

        // PUT: Nur Vorname, Nachname und Position werden übernommen. Id und Datumswerte bleiben erhalten.
        CreateMap<MitarbeiterAktualisierenDto, Mitarbeiter>()
            .ForMember(ziel => ziel.MitarbeiterId, optionen => optionen.Ignore())
            .ForMember(ziel => ziel.Geburtsdatum, optionen => optionen.Ignore())
            .ForMember(ziel => ziel.Einstellungsdatum, optionen => optionen.Ignore());

        CreateMap<Mitarbeiter, MitarbeiterAntwortDto>()
            .ForMember(
                ziel => ziel.VollerName, 
                optionen => optionen.MapFrom(quelle => $"{quelle.Vorname} {quelle.Nachname}")
            );
    }
}


