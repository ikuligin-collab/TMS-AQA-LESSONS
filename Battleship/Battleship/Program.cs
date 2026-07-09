using System.Net.Sockets;

class Program
{
    public static void Main()
    {
        try
        {
            var shipPosition = new Position(1, 1);
        
            var ship = new Ship(shipPosition, 2);
        
            var board = new Board(5, 5, new Ship[] { ship });
        
            Game game = new Game();
        
            game.Play(board);
        }
        catch (ShotPositionOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
        catch (FormatException ex)
        {
            Console.WriteLine(ex.Message);
        }
        finally
        {
           Console.WriteLine("Press any key to exit...");
           Console.ReadKey();
        }

        //
        // int[] myIntArray = new int[5];
        // int[] myIntArray2 = new int[] { 5, 4, 3, 6, 7 };
        //
        // myIntArray2[0] = 9;
        //
        // Console.WriteLine(myIntArray2[0]);
        //
        // for (int i = 0; i < myIntArray2.Length; i++)
        // {
        //     Console.WriteLine(myIntArray2[i]);
        // }
        //
        // Console.WriteLine(myIntArray2.Length);
        //
        // foreach (var intElement in myIntArray2)
        // {
        //     Console.WriteLine(intElement);
        // }
        //
        //
        // var list = myIntArray2.ToList();
        //
        // var myArray = list.ToArray();
        //
        // Console.WriteLine(list.Count);
        //
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
    public Position Position { get; }
    public int Length { get; }

    public Ship(Position position, int length)
    {
        Position = position;
        Length = length;
    }
}

class Board
{
    public int Rows { get; }
    public int Columns { get; }

    public Ship[] Ships { get; } // null

    public Board(int rows, int columns, Ship[] ships)
    {
        Rows = rows;
        Columns = columns;
        Ships = ships;
    }

    public bool IsInside(Position position)
    {
        return position.X >= 0 && position.X < Rows && position.Y >= 0 && position.Y < Columns;
    }

    public bool HasShip(Position position)
    {
        foreach (var currentShip in Ships)
        {
            if (position.Y == currentShip.Position.Y && position.X >= currentShip.Position.X &&
                position.X < currentShip.Position.X + currentShip.Length)
            {
                return true;
            }
        }

        // Ships.Any(currentShip => position.Y  == currentShip.Position.Y && position.X >= currentShip.Position.X && position.X < currentShip.Position.X + currentShip.Length); // есть ли хоть один элемент коллекции который отвечает заданному условию?
        //
        // var result = Ships.Where(currentShip => currentShip.Position.X == 0).ToArray(); // получить все элементы коллекции которые отвечают заданному условию
        //
        // var sh = Ships.Single(x => x.Position.Y == 2); // получить первый элемент коллекции который отвечает задонному условию, или верни null

        return false;
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
            if (!TryReadFromConsole("X", roundCount, out var xPosition))
                continue;

            Console.WriteLine();

            if (!TryReadFromConsole("Y", roundCount, out var yPosition))
                continue;

            if (xPosition == null || yPosition == null)
                continue;

            var shotPosition = new Position(xPosition.Value, yPosition.Value);

            if (!board.IsInside(shotPosition))
            {
                throw new ShotPositionOutOfRangeException("Invalid shot position!");
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

    private bool TryReadFromConsole(string coordinateName, int roundCount, out int? coordinate)
    {
        Console.WriteLine($"Input your {coordinateName} coordinate for round {roundCount}:");
        var input = Console.ReadLine();
        coordinate = null;

        //start business operation

        try
        {
            coordinate = int.Parse(input); // ArgumentNullException
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine("Invalid input!");
        }

        return true;
        // if (!int.TryParse(input, out coordinate))
        // {
        //     Console.WriteLine("Invalid input");
        //     return false;
        // }
    }
}

class ShotPositionOutOfRangeException : Exception
{
    public ShotPositionOutOfRangeException() : base()
    {
    }

    public ShotPositionOutOfRangeException(string message) : base(message)
    {
    }

    public ShotPositionOutOfRangeException(string message, Exception inner) : base(message, inner)
    {
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