using GameScoresApi.Data;
using GameScoresApi.Dtos.Score;
using GameScoresApi.Interfaces;
using GameScoresApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameScoresApi.Repositories;

public class ScoreRepository(ApplicationDbContext context) : IScoreRepository
{
    public async Task<List<Score>> GetAllAsync() =>
        await context.Scores
            .ToListAsync();
    
    public async Task<Score?> GetByIdAsync(int id) =>
        await context.Scores.FindAsync(id);

    public async Task<Score> CreateAsync(Score scoreModel)
    {
        await context.Scores.AddAsync(scoreModel);
        await context.SaveChangesAsync();
        return scoreModel;
    }

    public async Task<Score?> UpdateAsync(int scoreId, UpdateScoreDto scoreSto)
    {
        var existingScore = await context.Scores.FindAsync(scoreId);
        if (existingScore == null) return null;

        existingScore.Value = scoreSto.Value;
        existingScore.GameMode = scoreSto.GameMode;
        existingScore.ModifiedAt = DateTime.Now;
        existingScore.PlayerId = scoreSto.PlayerId;
        
        await context.SaveChangesAsync();
        return existingScore;
    }

    public async Task<Score?> DeleteAsync(int scoreId)
    {
        var scoreModel = await context.Scores.FindAsync(scoreId);
        if (scoreModel == null) return null;
        context.Scores.Remove(scoreModel);
        await context.SaveChangesAsync();
        return scoreModel;
    }

    public async Task<List<Score>> GetLeaderboardAsync() =>
        await context.Scores
            .Include(s => s.Player)
            .OrderByDescending(s => s.Value)
            .ToListAsync();
    
    public async Task<List<Score>> GetPlayerScoresAsync(int id) =>
        await context.Scores
            .Where(s => s.PlayerId == id)
            .ToListAsync();
    
    public async Task<Score?> GetPlayerHighestScoreAsync(int id) =>
        await context.Scores
            .Where(s => s.PlayerId == id)
            .OrderByDescending(s => s.Value)
            .FirstOrDefaultAsync();
    
    public async Task<int> ResetPlayerScoresAsync(int id) =>
        await context.Scores
            .Where(s => s.PlayerId == id)
            .ExecuteDeleteAsync();
}