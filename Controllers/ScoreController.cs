using GameScoresApi.Data;
using GameScoresApi.Dtos.Score;
using GameScoresApi.Interfaces;
using GameScoresApi.Mappers;
using Microsoft.AspNetCore.Mvc;

namespace GameScoresApi.Controllers;

[Route("api/scores")]
[ApiController]
public class ScoreController(ApplicationDbContext context, IScoreRepository scoreRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<ScoreDto>>> GetAllScores() =>
        Ok((await scoreRepository.GetAllAsync())
            .Select(s => s.ToScoreDto())
            .ToList());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<ScoreDto>> GetScoreById([FromRoute] int id) =>
        id <= 0 
            ? BadRequest("Invalid score ID")
            : (await scoreRepository.GetByIdAsync(id)) is { } score
            ? Ok(score.ToScoreDto())
            : NotFound("Score not found");

    [HttpPost]
    public async Task<ActionResult<ScoreDto>> CreateScore([FromBody] CreateScoreDto createScoreDto)
    {
        // TODO: Factor out the database operations for the player
        var player = await context.Players.FindAsync(createScoreDto.PlayerId);
        if (player == null) return NotFound("Player not found");
        
        var scoreModel = createScoreDto.FromCreateScoreDto(player);
        await scoreRepository.CreateAsync(scoreModel);
        return CreatedAtAction(nameof(GetScoreById), new { id = scoreModel.Id }, scoreModel.ToScoreDto());
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<ScoreDto>> UpdateScore([FromRoute] int id, [FromBody] UpdateScoreDto updateScoreDto)
    {
        if (id <= 0) return BadRequest("Invalid score ID");
        
        // TODO: Factor out the database operations for the player
        var player = await context.Players.FindAsync(updateScoreDto.PlayerId);
        if (player == null) return NotFound("Player not found");
        
        return (await scoreRepository.UpdateAsync(id, updateScoreDto)) is { } score
            ? Ok(score.ToScoreDto())
            : NotFound("Score not found");
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteScore([FromRoute] int id) =>
        id <= 0 
            ? BadRequest("Invalid score ID")
            : (await scoreRepository.DeleteAsync(id)) is not null
                ? NoContent()
                : NotFound("Score not found");

    [HttpGet("leaderboard")]
    public async Task<ActionResult<List<LeaderboardScoreDto>>> GetLeaderboard() =>
        Ok((await scoreRepository.GetLeaderboardAsync())
            .Select(s => s.ToLeaderboardScoreDto())
            .ToList());

    [HttpGet("players/{id:int}")]
    public async Task<ActionResult<List<ScoreDto>>> GetScoresByPlayer([FromRoute] int id) =>
        id <= 0 
            ? BadRequest("Invalid player ID") 
            : Ok((await scoreRepository.GetPlayerScoresAsync(id))
                .Select(s => s.ToScoreDto())
                .ToList());

    [HttpGet("players/{id:int}/highscore")]
    public async Task<ActionResult<ScoreDto>> GetHighScoreByPlayer([FromRoute] int id) => 
        id <= 0 
            ? BadRequest("Invalid player ID") 
            : (await scoreRepository.GetPlayerHighestScoreAsync(id)) is { } score
                ? Ok(score.ToScoreDto())
                : NotFound("Score not found");

    [HttpDelete("players/{id:int}/reset")]
    public async Task<ActionResult> ResetPlayerScore([FromRoute] int id) =>
        id <= 0 
            ? BadRequest("Invalid player ID")
            : (await scoreRepository.ResetPlayerScoresAsync(id)) == 0 
                ? NotFound("No scores found for the given player ID")
                : NoContent();
}