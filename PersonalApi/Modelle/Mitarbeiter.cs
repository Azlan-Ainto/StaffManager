using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace PersonalApi.Modelle
{
    public class Mitarbeiter
    {       
         public int MitarbeiterId { get; set; }

        [Required(ErrorMessage = "Vorname ist ein Pflichtfeld.")]
        public string Vorname { get; set; } = string.Empty;

        [Required(ErrorMessage = "Nachname ist ein Pflichtfeld")]
        public string Nachname { get; set; } = string.Empty;

        public string Position { get; set; } = string.Empty;

        public DateTime Einstellungsdatum { get; set; } = DateTime.Now;

        
    }
}
