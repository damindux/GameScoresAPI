using System.ComponentModel.DataAnnotations;

namespace GameScoresApi.Models;

public class Score
{
    public int Id { get; init; }
    public required int Value { get; set; }

    [MaxLength(10)] public required string GameMode { get; set; }

    public DateTime CreatedAt { get; init; }
    public DateTime ModifiedAt { get; set; }

    public required int PlayerId { get; set; }
    public required Player Player { get; init; }
}