namespace Battleship;

public class Board
{
    public int Rows { get; init;  }
    public int Columns { get; }

    public Ship[] Ships { get; } // null

    public Board(int rows, int columns, Ship[] ships)
    {
        // проверка что ращмеры доски не нулевые
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Размеры доски должны быть больше нуля.");
        this.Rows = rows;
        this.Columns = columns;
        this.Ships = ships;
        foreach (var ship in ships)
        {
            // Сначала проверяем, что начальная точка вообще на доске
            if (!IsInside(ship.Position))
            {
                throw new ArgumentException("Начало корабля находится вне доски!", nameof(ships));
            }

            // Проверяем хвост корабля в зависимости от его типа
            if (ship is HorizontalShip && ship.Position.X + ship.Length > Columns)
            {
                throw new ArgumentException("Горизонтальный корабль выходит за границы доски!", nameof(ships));
            }
            else if (ship is VerticalShip && ship.Position.Y + ship.Length > Rows)
            {
                throw new ArgumentException("Вертикальный корабль выходит за границы доски!", nameof(ships));
            }
        }
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