class Program
{
    public static void Main() 
    {
        //создаём доску игрока
        var shipPosition = new Position(2, 1);

        var ship = new Ship(shipPosition, 2); //x123234

        var board = new Board(5, 5, ship);

        var game = new Game();

        game.Play(board);
    }
}


class Position
{
    public int X { get; set; }
    public int Y { get; }

    public Position(int x, int y)
    {
        X = x;
        Y = y;
    }
}

class Ship
{
    // Координаты самой левой верней палубы
    public Position Position { get; } //null
    public int Length { get; } //12

    public Ship(Position position, int length)
    {
        if (length <= 0)
            throw new ArgumentException("Длина корабля должна быть больше 0!");
        Position = position;
        Length = length;
    }
}

class Board
{
    public int Rows { get; }
    public int Columns { get; }

    public Ship Ship { get; }

    public Board(int rows, int columns, Ship ship)
    {
        if (rows <= 0 || columns <= 0)
            throw new ArgumentException("Размеры доски должны быть больше нуля.");
        Rows = rows;
        Columns = columns;
        Ship = ship;
        if (!IsInside(ship.Position) || ship.Position.X + ship.Length > Rows)
        {
            throw new ArgumentOutOfRangeException(nameof(ship), "Корабль выходит за границы доски!");
        }
    }

    public bool IsInside(Position position)
    {
        return position.X >= 0 && position.X < Rows && position.Y >= 0 && position.Y < Columns;
    }

    public bool HasShip(Position position)
    {
        return position.Y == Ship.Position.Y && position.X >= Ship.Position.X &&
               position.X < Ship.Position.X + Ship.Length;
    }
}

class Game
{
    public void Play(Board board)
    {
        var roundCount = 0;
        while (true)
        {
            roundCount++;
            if (!TryReadFromConsole("X",  roundCount,  out var xPosition))
                continue;

            Console.WriteLine();

            if (!TryReadFromConsole("Y",  roundCount, out var yPosition))
                continue;

            var shotPosition = new Position(xPosition, yPosition);

            if (!board.IsInside(shotPosition))
            {
                Console.WriteLine("Invalid shot position!");
                continue;
            }

            if (board.HasShip(shotPosition))
            {
                Console.WriteLine("Hit!");
            }
            else
            {
                Console.WriteLine("Miss!");
            }
        }
    }

    private bool TryReadFromConsole(string coordinateName, int roundCount, out int coordinate)
    {
        Console.WriteLine($"Input your {coordinateName} coordinate for round {roundCount}:");
        var input = Console.ReadLine();
        if (!int.TryParse(input, out coordinate))
        {
            Console.WriteLine("Invalid input");
            return false;
        }
        
        return true;
    }
}

// Ship
// Board 
// Position
// Game

// Rows = 5

// 0 1 2 3 4 
// ------------X
// X X X X X 
// X X S S X   
// X X X X X 
// X X X X X 
// X X X X X 
// Y