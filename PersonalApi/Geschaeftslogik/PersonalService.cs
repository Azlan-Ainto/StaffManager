using AutoMapper;
using PersonalApi.DTOs;
using PersonalApi.Modelle;
using PersonalApi.Repositories;
using System.Collections.Generic;

namespace PersonalApi.Geschaeftslogik
{
    public class PersonalService : IPersonalService
    {
        private readonly IPersonalRepository _repository;
        private readonly IMapper _mapper;

        public PersonalService(IPersonalRepository repo , IMapper mapper) 
        {
            _repository = repo;
            _mapper = mapper;
        
        }



        public async Task<MitarbeiterAntwortDto> Mitarbeiter_Anlegen_Async(
            MitarbeiterErstellenDto neuerArbeiter
        )
        {

            int alter = DateTime.Today.Year - neuerArbeiter.Geburtsdatum.Year;

            if (neuerArbeiter.Geburtsdatum.Date > DateTime.Today.AddYears(-alter))
            {
                alter--;
            }

            if (alter < 18)
            {
                throw new ArgumentException("Geschäftsregel verletzt: " +
                    "Der Mitarbeiter muss mindestens 18 Jahre alt sein.");
            }

            var neuerkollege = _mapper.Map<Mitarbeiter>(neuerArbeiter);
            
            await _repository.Mitarbeiter_Hinzufuegen_Async(neuerkollege);

            // Speichern, bevor die Antwort erstellt wird:
            // Die MitarbeiterId vergibt SQL Server erst beim Speichern.
            await _repository.Aktualisieren_Async();

            return _mapper.Map<MitarbeiterAntwortDto>(neuerkollege);


        }
        //}

        public async Task<PaginierteAntwortDto<MitarbeiterAntwortDto>> Suche_Und_Paginiere_Mitarbeiter_Async(
            MitarbeiterSuchParameterDto parameter
        )
        {
            var (elemente, gesamtAnzahl) = await _repository.Suche_Und_Paginiere_Mitarbeiter_Async(parameter);
            
            var gemappteElemente = _mapper.Map<List<MitarbeiterAntwortDto>>(elemente);

            return new PaginierteAntwortDto<MitarbeiterAntwortDto>
            {
                Elemente = gemappteElemente,
               

            };

        }
    }
}
