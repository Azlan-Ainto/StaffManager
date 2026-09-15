using System.ComponentModel.DataAnnotations;

namespace PersonalApi.DTOs;

public class MitarbeiterAktualisierenDto
{
    [Required]
    public int MitarbeiterId { get; set; }
    [Required]
    public string Vorname { get; set; } = string.Empty;
    [Required]
    public string Nachname {  get; set; } = string.Empty;
    public string Position {  get; set; } = string.Empty;

}
