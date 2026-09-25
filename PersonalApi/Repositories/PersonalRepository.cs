using Microsoft.EntityFrameworkCore;
using PersonalApi.Datenbank;
using PersonalApi.DTOs;
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


        public async Task<(IEnumerable<Mitarbeiter> elemente, int Gesamtzahl)> Suche_Und_Paginiere_Mitarbeiter_Async(MitarbeiterSuchParameterDto suchParameterDto)
        {
            
            IQueryable<Mitarbeiter> abfrage = datenbankKontext.Mitarbeiter.AsQueryable();
            
            // 1. Filtern

            if(!string.IsNullOrWhiteSpace(suchParameterDto.Suchbegriff))
            {
                abfrage = abfrage.Where(m => m.Nachname.Contains(suchParameterDto.Suchbegriff)|| m.Vorname.Contains(suchParameterDto.Suchbegriff));
            }

            if (!string.IsNullOrWhiteSpace(suchParameterDto.Position))
            { 
                abfrage = abfrage.Where(m => m.Position == suchParameterDto.Position);

            }

            // 2. Sortieren

            switch (suchParameterDto.Sortierfeld) 
            {

                case "nachname":

                    abfrage = suchParameterDto.Aufteigend ? abfrage.OrderBy(m => m.Nachname) : abfrage.OrderByDescending(m => m.Vorname);

                    break;

                case "einstellungsdatum":

                    abfrage = suchParameterDto.Aufteigend ? abfrage.OrderBy(m =>m.Einstellungsdatum) : abfrage.OrderByDescending(m =>m.Einstellungsdatum);
                    
                    break;

                default:

                    abfrage = suchParameterDto.Aufteigend ? abfrage.OrderBy(m => m.MitarbeiterId) : abfrage.OrderByDescending(m =>m.MitarbeiterId);
                    break;
            
            }
            // 3.Zählen

            int gesamtZahl = await abfrage.CountAsync();

            // 4.Paginieren

            int ueberspringen = (suchParameterDto.Seite - 1) * suchParameterDto.SeitenGroesse;

            // 5. Daten aus Datenbank laden

            var ergebnisse = await abfrage.ToListAsync();

            return (ergebnisse, gesamtZahl);


        }
    }
}
