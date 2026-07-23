namespace Battleship;

public record Shot(Board Board, Position Position, Ship? Ship)
{
    public ShootResult Result => Ship != null ? ShootResult.Hit : ShootResult.Miss;
}