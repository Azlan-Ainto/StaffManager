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

        public async Task<IEnumerable<Mitarbeiter>> Suche_Mitarbeiter_Async(string suchbegriff, string position)
        {

            IQueryable<Mitarbeiter> abfrage = datenbankKontext.Mitarbeiter.AsQueryable();


            if (!string.IsNullOrWhiteSpace(suchbegriff))

            {
                abfrage = abfrage.Where(m => m.Vorname.Contains(suchbegriff) || m.Nachname.Contains(suchbegriff));

            }

            if(!string.IsNullOrWhiteSpace(position))
            {
                abfrage = abfrage.Where(m => m.Position == position);
            }
            // Deferred Execution (verzögerte Ausführung)

            return await abfrage.ToListAsync();
        }
    }
}
