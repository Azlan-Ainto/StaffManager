using Microsoft.EntityFrameworkCore;
using PersonalApi.Modelle;

namespace PersonalApi.Datenbank
{
    public class PersonalKontext: DbContext
    {
        public DbSet<Mitarbeiter> Mitarbeiter { get; set; }


        public PersonalKontext(DbContextOptions<PersonalKontext> options) : base(options) 
        { 

        }
    }
}
