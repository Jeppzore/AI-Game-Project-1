using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class CreateObstacleRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; } = string.Empty;

    public string Difficulty { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public string Location { get; set; } = string.Empty;

    [StringLength(2000)]
    public string SolveMethod { get; set; } = string.Empty;
}

public class UpdateObstacleRequest
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Difficulty { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public string? Location { get; set; }

    [StringLength(2000)]
    public string? SolveMethod { get; set; }
}
