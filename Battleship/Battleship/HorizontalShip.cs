namespace Battleship;

public class HorizontalShip : Ship
{
    public HorizontalShip(Position position, int length) : base(position, length) { }
    
    public override bool IsOnPosition(Position position)
    {
        return position.Y == Position.Y && position.X >= Position.X && position.X < Position.X + Length;
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