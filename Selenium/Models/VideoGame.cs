using System.ComponentModel.DataAnnotations;

namespace Selenium.Models;

public class VideoGame
{
    public int Id { get; set; }

    [Required(ErrorMessage = "El título es obligatorio")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "El título debe tener entre 2 y 100 caracteres")]
    public string Title { get; set; } = string.Empty;

    [Required(ErrorMessage = "El género es obligatorio")]
    public string Genre { get; set; } = string.Empty;

    [Required(ErrorMessage = "La plataforma es obligatoria")]
    public string Platform { get; set; } = string.Empty;

    [Range(0, 9999, ErrorMessage = "El precio debe estar entre 0 y 9999")]
    public decimal Price { get; set; }

    [Range(0, 1000, ErrorMessage = "El stock debe estar entre 0 y 1000")]
    public int Stock { get; set; }
}