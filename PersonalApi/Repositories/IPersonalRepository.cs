using PersonalApi.Modelle;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace PersonalApi.Repositories
{
    public interface IPersonalRepository
    {
        Task<IEnumerable<Mitarbeiter>> Hole_Alle_Mitarbeiter_Async();
        Task<Mitarbeiter?> Hole_Mitarbeiter_Nach_Id_Async(int d);
        Task Mitarbeiter_Hinzufuegen_Async(Mitarbeiter mitarbeiter);
        Task Aktualisieren_Async();
        Task Mitarbeiter_Loeschen_Async(Mitarbeiter mitarbeiter);

    }
}
