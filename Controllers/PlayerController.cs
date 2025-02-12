using GameScoresApi.Dtos.Player;
using GameScoresApi.Interfaces;
using GameScoresApi.Mappers;
using GameScoresApi.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace GameScoresApi.Controllers;

[Route("api/players")]
[ApiController]
public class PlayerController(IPlayerRepository playerRepository) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<PlayerDto>>> GetAllPlayers() =>
        Ok((await playerRepository.GetAllAsync())
            .Select(p => p.ToPlayerDto())
            .ToList());
    
    [HttpGet("{id:int}")]
    public async Task<ActionResult<PlayerDto>> GetPlayerById([FromRoute] int id) =>
        id <= 0 
            ? BadRequest("Invalid player ID") 
            : (await playerRepository.GetByIdAsync(id)) is { } player
                ? Ok(player.ToPlayerDto())
                : NotFound("Player not found");

    [HttpPost]
    public async Task<ActionResult<PlayerDto>> AddPlayer([FromBody] CreatePlayerDto createPlayerDto)
    {
        var playerModel = createPlayerDto.FromCreatePlayerDto();
        await playerRepository.CreateAsync(playerModel);
        return CreatedAtAction(nameof(GetPlayerById), new { id = playerModel.Id }, playerModel);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<PlayerDto>> UpdatePlayer([FromRoute] int id,
        [FromBody] UpdatePlayerDto updatePlayerDto) =>
        id <= 0 
            ? BadRequest("Invalid player ID")
            : (await playerRepository.UpdateAsync(id, updatePlayerDto)) is { } player
                ? Ok(player.ToPlayerDto())
                : NotFound("Player not found");
    
    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeletePlayer([FromRoute] int id) =>
        id <= 0
            ? BadRequest("Invalid player ID")
            : (await playerRepository.DeleteAsync(id)) is not null
                ? NoContent()
                : NotFound("Player not found");
    
}