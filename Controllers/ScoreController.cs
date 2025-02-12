using GameScoresApi.Data;
using GameScoresApi.Dtos.Score;
using GameScoresApi.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace GameScoresApi.Controllers;

[Route("api/scores")]
[ApiController]
public class ScoreController(ApplicationDbContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ScoreDto>>> GetAllScores() => 
        Ok(await context.Scores
            .Select(s => s.ToScoreDto())
            .ToListAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScoreDto>> GetScoreById([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("Invalid score ID");
        
        var score = await context.Scores.FindAsync(id);
        
        if (score == null) return NotFound("Score not found");
        return Ok(score.ToScoreDto());
    }

    [HttpPost]
    public async Task<ActionResult<ScoreDto>> CreateScore([FromBody] CreateScoreDto createScoreDto)
    {
        var player = await context.Players.FindAsync(createScoreDto.PlayerId);
        if (player == null) return NotFound("Player not found");
        var scoreModel = createScoreDto.FromCreateScoreDto(player);
        context.Scores.Add(scoreModel);
        await context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetScoreById), new { id = scoreModel.Id }, scoreModel.ToScoreDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScoreDto>> UpdateScore([FromRoute] int id, [FromBody] UpdateScoreDto updateScoreDto)
    {
        if (id <= 0) return BadRequest("Invalid score ID");
        
        var scoreModel = await context.Scores.FirstOrDefaultAsync(x => x.Id == id);
        if (scoreModel == null) return NotFound("Score not found");
        
        var player = await context.Players.FindAsync(updateScoreDto.PlayerId);
        if (player == null) return NotFound("Player not found");

        scoreModel.Value = updateScoreDto.Value;
        scoreModel.GameMode = updateScoreDto.GameMode;
        scoreModel.ModifiedAt = DateTime.Now;
        scoreModel.PlayerId = updateScoreDto.PlayerId;
        
        await context.SaveChangesAsync();
        return Ok(scoreModel.ToScoreDto());
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteScore([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("Invalid score ID");
        
        var scoreModel = await context.Scores.FirstOrDefaultAsync(x => x.Id == id);
        
        if (scoreModel == null) return NotFound("Score not found");
        
        context.Scores.Remove(scoreModel);
        await context.SaveChangesAsync();
        return NoContent();
    }

    [HttpGet("leaderboard")]
    public async Task<ActionResult<List<LeaderboardScoreDto>>> GetLeaderboard() =>
        Ok(await context.Scores
            .Include(s => s.Player)
            .OrderByDescending(s => s.Value)
            .Select(s => s.ToLeaderboardScoreDto())
            .ToListAsync());

    [HttpGet("players/{id:int}")]
    public async Task<ActionResult<List<ScoreDto>>> GetScoresByPlayer([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("Invalid player ID");
        
        var scores = await context.Scores
            .Where(s => s.PlayerId == id)
            .Select(s => s.ToScoreDto())
            .ToListAsync();
        
        return Ok(scores);
    }

    [HttpGet("players/{id:int}/highscore")]
    public async Task<ActionResult<ScoreDto>> GetHighScoreByPlayer([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("Invalid player ID");
        
        var score = await context.Scores
            .Where(s => s.PlayerId == id)
            .OrderByDescending(s => s.Value)
            .Select(s => s.ToScoreDto())
            .FirstOrDefaultAsync();
        
        if (score == null) return NotFound("Score not found");
        return Ok(score);
    }

    [HttpDelete("players/{id:int}/reset")]
    public async Task<ActionResult> ResetPlayerScore([FromRoute] int id)
    {
        if (id <= 0) return BadRequest("Invalid player ID");

        var affectedRows = await context.Scores
            .Where(s => s.PlayerId == id)
            .ExecuteDeleteAsync();

        if (affectedRows == 0) return NotFound("No scores found for the given player ID");
        return NoContent();
    }
}