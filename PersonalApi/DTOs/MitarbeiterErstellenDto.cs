using System.ComponentModel.DataAnnotations;

namespace PersonalApi.DTOs;

public class MitarbeiterErstellenDto
{
    [Required(ErrorMessage = "Vorname ist ein Pflichtfeld.")]
    public string Vorname { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Nachname ist ein Pflichtfeld.")]
    public string Nachname { get; set; } = string.Empty;
    
    public string Position { get; set; } = string.Empty;

    [Required(ErrorMessage ="Das Geburtsdatum ist zwingen erforderlich.")]
    public DateTime Geburtsdatum { get; set;  }

}
