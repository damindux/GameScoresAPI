using GameScoresApi.Data;
using GameScoresApi.Dtos.Player;
using GameScoresApi.Interfaces;
using GameScoresApi.Models;
using Microsoft.EntityFrameworkCore;

namespace GameScoresApi.Repositories;

public class PlayerRepository(ApplicationDbContext context) : IPlayerRepository
{
    public async Task<List<Player>> GetAllAsync() =>
        await context.Players.ToListAsync();

    public async Task<Player?> GetByIdAsync(int id) =>
        await context.Players.FindAsync(id);

    public async Task<Player> CreateAsync(Player player)
    {
        await context.Players.AddAsync(player);
        await context.SaveChangesAsync();
        return player;
    }

    public async Task<Player?> UpdateAsync(int playerId, UpdatePlayerDto playerDto)
    {
        var existingPlayer = await context.Players.FindAsync(playerId);
        if (existingPlayer == null) return null;

        existingPlayer.Name = playerDto.Name;
        existingPlayer.ModifiedAt = DateTime.Now;
        
        await context.SaveChangesAsync();
        return existingPlayer;
    }

    public async Task<Player?> DeleteAsync(int id)
    {
        var player = await context.Players.FindAsync(id);
        if (player == null) return null;
        context.Players.Remove(player);
        await context.SaveChangesAsync();
        return player;
    }
}