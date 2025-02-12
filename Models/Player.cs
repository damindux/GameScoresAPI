using System.ComponentModel.DataAnnotations;

namespace GameScoresApi.Models;

public class Player
{
    public int Id { get; init; }
    [MaxLength(15)] public required string Name { get; set; }
    public DateTime CreatedAt { get; init; }
    public DateTime ModifiedAt { get; set; }
    
    public List<Score> Scores { get; set; } = [];
}