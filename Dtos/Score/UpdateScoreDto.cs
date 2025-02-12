namespace GameScoresApi.Dtos.Score;

public class UpdateScoreDto
{
    public int Id { get; set; }
    public required int Value { get; set; }
    public required string GameMode { get; set; }
    public DateTime ModifiedAt { get; set; }
    public required int PlayerId { get; set; }
}