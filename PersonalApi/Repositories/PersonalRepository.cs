using Microsoft.EntityFrameworkCore;
using PersonalApi.Datenbank;
using PersonalApi.Modelle;

namespace PersonalApi.Repositories
{
    public class PersonalRepository : IPersonalRepository
    {
        private readonly PersonalKontext datenbankKontext;

        public PersonalRepository(PersonalKontext kontext)
        {
            datenbankKontext = kontext;

        }

        public async Task Aktualisieren_Async()
        {
            await datenbankKontext.SaveChangesAsync();
        }

        public async Task<IEnumerable<Mitarbeiter>> Hole_Alle_Mitarbeiter_Async( )
        {
            return await datenbankKontext.Mitarbeiter.ToListAsync();
        }

        public async Task<Mitarbeiter?> Hole_Mitarbeiter_Nach_Id_Async(int id)
        {
            return await datenbankKontext.Mitarbeiter.FirstOrDefaultAsync(m => m.MitarbeiterId == id);

        }

        public async Task Mitarbeiter_Hinzufuegen_Async(Mitarbeiter arbeiter)
        {
            await datenbankKontext.Mitarbeiter.AddAsync(arbeiter);

        }
        public async Task Mitarbeiter_Loeschen_Async(Mitarbeiter mitarbeiter)
        {
            datenbankKontext.Mitarbeiter.Remove(mitarbeiter);
        }
    }
}
