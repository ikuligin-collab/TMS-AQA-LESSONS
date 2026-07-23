namespace Battleship;

public class VerticalShip : Ship
{
    public VerticalShip(Position position, int length) : base(position, length) { }
    
    public override bool IsOnPosition(Position shotPosition)
    {
        return shotPosition.X == Position.X && shotPosition.Y >= Position.Y && shotPosition.Y < Position.Y + Length;
    }

    public override bool IntersectsWith(Ship other)
    {
        foreach (var pos in GetPositions())
        {
            if (other.IsOnPosition(pos))
                return true;
        }
        return false;
    }
}