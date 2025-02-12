using GameScoresApi.Dtos.Player;
using GameScoresApi.Models;

namespace GameScoresApi.Interfaces;

public interface IPlayerRepository
{
    Task<List<Player>> GetAllAsync();
    Task<Player?> GetByIdAsync(int id);
    Task<Player> CreateAsync(Player player);
    Task<Player?> UpdateAsync(int playerId, UpdatePlayerDto playerDto);
    Task<Player?> DeleteAsync(int id);
}