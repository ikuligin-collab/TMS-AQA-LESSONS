namespace Battleship;

class HumanPlayer : IPlayer
{
    public string Name { get; set; } = "Игрок";
    
    public Shot Shoot(Board targetBoard)
    {
        int xPosition, yPosition;
        
        while (true)
        {
            if (TryReadFromConsole("X (столбец)", out xPosition) && TryReadFromConsole("Y (строка)", out yPosition))
            {
                var shotPosition = new Position(xPosition, yPosition);
                
                if (!targetBoard.IsInside(shotPosition))
                {
                    Console.WriteLine("Выстрел вне границ доски! Попробуйте еще раз.");
                    continue;
                }

                // Находим корабль на этой позиции (если есть)
                Ship? targetShip = targetBoard.Ships.FirstOrDefault(s => s.IsOnPosition(shotPosition));
                return new Shot(targetBoard, shotPosition, targetShip);
            }
            Console.WriteLine("Некорректный ввод чисел. Попробуйте еще раз.");
        }
    }
    
    private bool TryReadFromConsole(string coordinateName, out int coordinate)
    {
        Console.Write($"Введите координату {coordinateName}: ");
        var input = Console.ReadLine();
        return int.TryParse(input, out coordinate);
    }

    public void WriteName()
    {
        Console.WriteLine($"Игрок: {Name}");
    }
}