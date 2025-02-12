namespace GameScoresApi.Dtos.Player;

public class UpdatePlayerDto
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public DateTime ModifiedAt { get; set; }
}