namespace Battleship;

class Board
{
    public int Rows { get; init;  }
    public int Columns { get; }

    public Ship[] Ships { get; } // null

    public Board(int rows, int columns, Ship[] ships)
    {
        this.Rows = rows;
        this.Columns = columns;
        this.Ships = ships;
    }

    public bool IsInside(Position position) //  ref if Positions is a class, value if Positions is a struct
    {
        return position.X >= 0 && position.X < Columns && position.Y >= 0 && position.Y < Rows;
    }

    public bool HasShip(Position position)
    {
        return Ships.Any(currentShip => currentShip.IsOnPosition(position));
    }
    
    private void PrivateLogic()
    {
    }
}