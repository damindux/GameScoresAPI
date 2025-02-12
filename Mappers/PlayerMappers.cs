using GameScoresApi.Dtos.Player;
using GameScoresApi.Models;

namespace GameScoresApi.Mappers;

public static class PlayerMappers
{
    public static PlayerDto ToPlayerDto(this Player player) =>
        new()
        {
            Id = player.Id,
            Name = player.Name,
            CreatedAt = player.CreatedAt,
            ModifiedAt = player.ModifiedAt,
        };

    public static Player FromCreatePlayerDto(this CreatePlayerDto playerDto) => new() { Name = playerDto.Name };
}