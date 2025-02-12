namespace GameScoresApi.Dtos.Score;

public class LeaderboardScoreDto
{
    public required string PlayerName { get; set; }
    public required int PlayerScore { get; set; }
}