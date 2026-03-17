using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class CreateCreatureRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 200 characters")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Type is required")]
    public string Type { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    [Range(1, 10000, ErrorMessage = "Health must be between 1 and 10000")]
    public int Health { get; set; }

    [Range(0, 10000, ErrorMessage = "Attack must be between 0 and 10000")]
    public int Attack { get; set; }

    [Range(0, 10000, ErrorMessage = "Defense must be between 0 and 10000")]
    public int Defense { get; set; }

    [Range(0, 100000, ErrorMessage = "Experience must be between 0 and 100000")]
    public int Experience { get; set; }

    public string Location { get; set; } = string.Empty;

    public List<string> Abilities { get; set; } = new();

    public List<string> Weaknesses { get; set; } = new();

    public List<LootDrop> Drops { get; set; } = new();
}

public class UpdateCreatureRequest
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }

    public string? Type { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    [Range(1, 10000)]
    public int? Health { get; set; }

    [Range(0, 10000)]
    public int? Attack { get; set; }

    [Range(0, 10000)]
    public int? Defense { get; set; }

    [Range(0, 100000)]
    public int? Experience { get; set; }

    public string? Location { get; set; }

    public List<string>? Abilities { get; set; }

    public List<string>? Weaknesses { get; set; }

    public List<LootDrop>? Drops { get; set; }
}
