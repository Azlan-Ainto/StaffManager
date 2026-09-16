using AutoMapper;
using PersonalApi.DTOs;
using PersonalApi.Modelle;
using PersonalApi.Repositories;

namespace PersonalApi.Geschaeftslogik
{
    public class PersonalService : IPersonalService
    {
        private readonly IPersonalRepository _repository;
        private readonly IMapper _mapper;
        public PersonalService(IPersonalRepository repository , IMapper automapper) 
        {
            _repository = repository;
            _mapper = automapper;
        
        }




        public async Task<MitarbeiterAntwortDto> Mitarbeiter_Anlegen_Async(MitarbeiterErstellenDto neuerArbeiter)
        {

            int alter = DateTime.Today.Year - neuerArbeiter.Geburtsdatum.Year;

            if (neuerArbeiter.Geburtsdatum.Date > DateTime.Today.AddYears(-alter))
            {
                alter--;
            }

            if (alter < 18)
            {
                throw new ArgumentException("Geschäftsregel verletzt: Der Mitarbeiter muss mindestens 18 Jahre alt sein.");
            }

            var neuerkollege = _mapper.Map<Mitarbeiter>(neuerArbeiter);
            
            await _repository.Mitarbeiter_Hinzufuegen_Async(neuerkollege);
                

            return _mapper.Map<MitarbeiterAntwortDto>(neuerkollege);


        }
    }
}
