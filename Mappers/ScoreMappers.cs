using GameScoresApi.Dtos.Score;
using GameScoresApi.Models;

namespace GameScoresApi.Mappers;

public static class ScoreMappers
{
    public static ScoreDto ToScoreDto(this Score score) =>
        new()
        {
            Id = score.Id,
            Value = score.Value,
            GameMode = score.GameMode,
            CreatedAt = score.CreatedAt,
            ModifiedAt = score.ModifiedAt,
            PlayerId = score.PlayerId
        };

    public static Score FromCreateScoreDto(this CreateScoreDto createScoreDto, Player player) =>
        new()
        {
            Value = createScoreDto.Value,
            GameMode = createScoreDto.GameMode,
            PlayerId = createScoreDto.PlayerId,
            Player = player
        };


    public static LeaderboardScoreDto ToLeaderboardScoreDto(this Score score) =>
        new()
        {
            PlayerName = score.Player.Name,
            PlayerScore = score.Value,
        };
}