using System.ComponentModel.DataAnnotations;

namespace Server.Models;

public class CreateNpcRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(200, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = string.Empty;

    [StringLength(2000)]
    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    public NPCLocation Location { get; set; } = new();

    public NPCTrades Trades { get; set; } = new();

    public List<NPCQuest> Quests { get; set; } = new();
}

public class UpdateNpcRequest
{
    [StringLength(200, MinimumLength = 1)]
    public string? Name { get; set; }

    public string? Role { get; set; }

    [StringLength(2000)]
    public string? Description { get; set; }

    public string? ImageUrl { get; set; }

    public NPCLocation? Location { get; set; }

    public NPCTrades? Trades { get; set; }

    public List<NPCQuest>? Quests { get; set; }
}
