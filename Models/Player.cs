using System.ComponentModel.DataAnnotations;

namespace GameScoresApi.Models;

public class Player
{
    public int Id { get; init; }
    
    [MaxLength(15)] public required string Name { get; set; }
    
    public List<Score> Scores { get; set; } = [];
}