using AutoMapper;
using PersonalApi.Modelle;
using PersonalApi.DTOs;

namespace PersonalApi.Zuordnung;

public class PersonalProfil : Profile
{
    public PersonalProfil()
    {
        CreateMap<MitarbeiterErstellenDto, Mitarbeiter>();
        CreateMap<Mitarbeiter, MitarbeiterAntwortDto>();

        CreateMap<Mitarbeiter, MitarbeiterAntwortDto>()
            .ForMember(
                ziel => ziel.VollerName, 
                optionen => optionen.MapFrom(quelle => $"{quelle.Vorname} {quelle.Nachname}")
            );
    }
}


