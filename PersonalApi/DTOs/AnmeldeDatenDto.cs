using System.ComponentModel.DataAnnotations;

namespace PersonalApi.DTOs
{
    public class AnmeldeDatenDto
    {

        [Required(ErrorMessage ="Benutzername ist erforderlich.")]
        public string Benutzername { get; set; } = string.Empty;

        [Required(ErrorMessage ="Password ist erforderlich.")]
        public string Password { get; set; } = string.Empty;
    }
}
