namespace GameScoresApi.Dtos.Score;

public class CreateScoreDto
{
    public required int Value { get; set; }
    public required string GameMode { get; set; }
    public required int PlayerId { get; set; }
}