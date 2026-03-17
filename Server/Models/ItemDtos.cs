using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class CreateItemRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; } = string.Empty;

    public string Category { get; set; } = string.Empty;

    public string Rarity { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public ItemStats Stats { get; set; } = new();

    public ItemRequirements Requirements { get; set; } = new();

    public ItemAcquisition Acquisition { get; set; } = new();

    public List<string> UsedBy { get; set; } = new();
}

public class UpdateItemRequest
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }

    public string? Type { get; set; }

    public string? Category { get; set; }

    public string? Rarity { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public ItemStats? Stats { get; set; }

    public ItemRequirements? Requirements { get; set; }

    public ItemAcquisition? Acquisition { get; set; }

    public List<string>? UsedBy { get; set; }
}
