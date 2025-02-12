using GameScoresApi.Dtos.Score;
using GameScoresApi.Models;

namespace GameScoresApi.Interfaces;

public interface IScoreRepository
{
    Task<List<Score>> GetAllAsync();
    Task<Score?> GetByIdAsync(int scoreId);
    Task<Score> CreateAsync(Score scoreModel);
    Task<Score?> UpdateAsync(int scoreId, UpdateScoreDto scoreDto);
    Task<Score?> DeleteAsync(int scoreId);
    Task<List<Score>> GetLeaderboardAsync();
    Task<List<Score>> GetPlayerScoresAsync(int playerId);
    Task<Score?> GetPlayerHighestScoreAsync(int playerId);
    Task<int> ResetPlayerScoresAsync(int playerId);
}