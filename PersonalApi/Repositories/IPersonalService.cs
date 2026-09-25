using PersonalApi.DTOs;
using PersonalApi.Modelle;

namespace PersonalApi.Repositories
{
    public interface IPersonalService
    {
        Task<MitarbeiterAntwortDto> Mitarbeiter_Anlegen_Async(MitarbeiterErstellenDto neuerArbeiter);

        //Task<IEnumerable<MitarbeiterAntwortDto>> Mitarbeiter_suchen_Async(string suchbegriff, string position);

        // Vorherige Methode überschreiben:
        Task<PaginierteAntwortDto<MitarbeiterAntwortDto>> Suche_Und_Paginiere_Mitarbeiter_Async(MitarbeiterSuchParameterDto parameter);

    }
}
