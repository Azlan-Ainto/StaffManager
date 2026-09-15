using System.ComponentModel.DataAnnotations;

namespace PersonalApi.DTOs;

public class MitarbeiterAntwortDto
{
    public int MitarbeiterId { get; set; }
    public string VollerName { get; set; } = string.Empty;
    public string Position {  get; set; } = string.Empty;
    public DateTime Einstellungsdatum { get; set; }
}
