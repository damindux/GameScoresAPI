namespace GameScoresApi.Dtos.Score;

public class ScoreDto
{
    public int Id { get; set; }
    public required int Value { get; set; }
    public required string GameMode { get; set; }
    public required DateTime CreatedAt { get; set; }
    public required DateTime ModifiedAt { get; set; }
    public required int PlayerId { get; set; }
}