using System;

class Program
{
    public static void Main() 
    {
        try
        {
            //создаём доску игрока
            var shipPosition = new Position(2, 1);

            var ship = new Ship(shipPosition, 2); //x123234

            var board = new Board(5, 5, ship);

            var game = new Game();

            game.Play(board);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка игры: {ex.Message}");
        }
        
    }
}


class Position
{
    public int X { get; }
    public int Y { get; }

    public Position(int x, int y)
    {
        //проверка на отрицательные координаты
        if (x < 0 || y < 0)
            throw new ArgumentException("Координаты не могут быть отрицательными");
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
        // Проверки параметров корабля
        //пустая позиция
        if (position == null)
            throw new ArgumentNullException(nameof(position), "Позиция корабля не может быть null.");
        //отрицательная позиция
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
        if (ship == null)
            throw new ArgumentNullException(nameof(ship), "Корабль не может быть null.");
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

    //  Метод поиска корабля по координатам выстрела
    public Ship FindShip(Position position)
    {
        if (!IsInside(position))
            throw new ArgumentOutOfRangeException(nameof(position), "Координаты находятся вне доски.");

        // Проверяем, задевает ли позиция наш корабль (по горизонтали X)
        bool hitsShip = position.Y == Ship.Position.Y && 
                        position.X >= Ship.Position.X && 
                        position.X < Ship.Position.X + Ship.Length;

        return hitsShip ? Ship : null;
    }
    public bool HasShip(Position position)
    {
        if (!IsInside(position))
            throw new ArgumentOutOfRangeException(nameof(position), "Попытка проверить клетку за пределами поля!");
        return position.Y == Ship.Position.Y && position.X >= Ship.Position.X &&
               position.X < Ship.Position.X + Ship.Length;
    }
}
//Класс для хранения истории выстрелов
class ShotRecord
{
    public string PlayerName { get; }
    public Position Position { get; }
    public bool IsHit { get;}

    public ShotRecord(string playerName, Position position, bool isHit)
    {
        PlayerName = playerName;
        Position = position;
        IsHit = isHit; 
    }
}
 //Класс выстрела
 class Shot
 {
     public Board Board { get; }
     public Position Position { get; }
     public Ship Ship { get; } // null, если промах

     public Shot(Board board, Position position, Ship ship)
     {
         Board = board ?? throw new ArgumentNullException(nameof(board));
         Position = position ?? throw new ArgumentNullException(nameof(position));
         Ship = ship; // может быть null
     }
 }
class Game
{
    private readonly Random _random = new Random();
    // свойства для хранения попаданий
    public int PlayerHits { get; private set; }
    public int OpponentHits { get; private set; }
    // создаю списокд для хранения истории выстрелов
    private readonly List<ShotRecord> _shotHistory = new List<ShotRecord>();
    //Создаю коллекцию 
    private readonly List<Shot> _shots = new List<Shot>();
    public void Play(Board playerBoard)
    {
        // Создаем доску компьютера
        Board opponentBoard = GenerateOpponentBoard(playerBoard.Rows, playerBoard.Columns);
        
        Console.WriteLine("Игра началась!Всем приготовится!!");
        var roundCount = 0;

        //Цикл игры не будет завершён, пока противник не уничтожит все палубы
        while (PlayerHits < opponentBoard.Ship.Length && OpponentHits < playerBoard.Ship.Length)
        {
            roundCount++;
            Console.WriteLine($"\n=== Раунд № {roundCount} ===");

            // --- ХОД ИГРОКА ---
            int xPosition, yPosition;
            while (!TryReadFromConsole("X", roundCount, out xPosition)) { }
            while (!TryReadFromConsole("Y", roundCount, out yPosition)) { }
           

            // обработка выстрела вне поля
            try
            {
                //Проверка что ивыстрел внутри поля
                var playerShot = new Position(xPosition, yPosition);
                //Переменная под сохранение истории выстрела игрока
                bool isPlayerHit = opponentBoard.HasShip(playerShot);
                
                // Сохранение выстрела в истории
                _shotHistory.Add(new ShotRecord("Игрок", playerShot, isPlayerHit));
                _shots.Add(new Shot(opponentBoard, playerShot, isPlayerHit ? opponentBoard.Ship : null));
                
                // Игрок стреляет по доске компъютера
                if (isPlayerHit)
                {
                    Console.WriteLine("Игрок: Попадание!");
                    PlayerHits++; 
                }
                else
                {
                    Console.WriteLine("Игрок: Промах!");
                }
            }
            catch (ArgumentOutOfRangeException ex)
            {
                // ошибка елси координаты вне достки
                Console.WriteLine($"Выстрел вне координат доски: {ex.Message} Попробуйте снова.");
                continue; 
            }
            
            
            // Если игрок уже победил, компьютер не ходит
            if (PlayerHits >= opponentBoard.Ship.Length) break;

            // ХОД КОМПЬЮТЕРА
            // случайный ход компьютера
            int compX = _random.Next(0, playerBoard.Rows);
            int compY = _random.Next(0, playerBoard.Columns);
            var compShot = new Position(compX, compY);
            
            // Вывод координат компьютера
            Console.Write($"Компьютер стреляет в координаты ({compX}, {compY}) -> ");
            
            //Переменная для хранения истории выстрела компьютера
            bool isCompHit = playerBoard.HasShip(compShot);
            
            // Сохранtybt выстрела компьютера в истории
            _shots.Add(new Shot(playerBoard, compShot, isCompHit ? playerBoard.Ship : null));

            // Компьютер стреляет по доске игрока
            if (isCompHit)
            {
                Console.WriteLine("Компьютер: Попадание!");
                OpponentHits++; // добавляем колчесвто попаданий компьютеру
            }
            else
            {
                Console.WriteLine("Компьютер: Промах!");
            }
            

            // вывод текущего счета
            Console.WriteLine($"Текущий счет после раунда {roundCount}: Игрок {PlayerHits} - {OpponentHits} Компьютер");
            // Вызываем метод, передавая ему обе доски
            DisplayRoundStatistics(playerBoard, opponentBoard);
        }
    }
   // Генерация достки противника
    private Board GenerateOpponentBoard(int rows, int columns)
    {
        while (true)
        {
            try
            {
                // Случайная длина корабля от 1 до 3 клеток
                int length = _random.Next(1, 4); 
                
                // Позиция Y может быть любой на доске
                int y = _random.Next(0, columns);
                
                // Позиция X должна быть такой, чтобы корабль по длине не вылез за Rows
                int x = _random.Next(0, rows - length + 1); 

                var compShip = new Ship(new Position(x, y), length);
                return new Board(rows, columns, compShip);
            }
            catch (Exception ex)
            {
                // Если вдруг алгоритм генерации выдаст ошибку, конструктор выбросит исключение
                Console.WriteLine($"Сбой генерации: {ex.Message}");
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
    // 8. Вывод статистики по каждой доске в конце раунда с использованием LINQ
    private void DisplayRoundStatistics(Board playerBoard, Board opponentBoard)
    {
        Console.WriteLine("\n--- СТАТИСТИКА РАУНДА (LINQ) ---");

        Board[] boards = { playerBoard, opponentBoard };
        foreach (var board in boards)
        {
            string boardOwner = board == playerBoard ? "ИГРОКА" : "КОМПЬЮТЕРА";
            Console.WriteLine($"\nДоска {boardOwner}:");

            // Получаем все выстрелы по этой конкретной доске
            var boardShots = _shots.Where(s => s.Board == board).ToList();

            // 8.1. Общее количество выстрелов
            int totalShots = boardShots.Count;
            Console.WriteLine($"  8.1. Всего выстрелов по доске: {totalShots}");

            // 8.2. Количество попаданий
            int hits = boardShots.Count(s => s.Ship != null);
            Console.WriteLine($"  8.2. Попаданий: {hits}");

            // 8.3. Количество промахов
            int misses = boardShots.Count(s => s.Ship == null);
            Console.WriteLine($"  8.3. Промахов: {misses}");

            // 8.4. Был ли хотя бы один промах
            bool hasMiss = boardShots.Any(s => s.Ship == null);
            Console.WriteLine($"  8.4. Был ли хоть один промах: {(hasMiss ? "Да" : "Нет")}");

            // 8.5. Первый успешный выстрел
            var firstHit = boardShots.FirstOrDefault(s => s.Ship != null);
            if (firstHit != null)
            {
                Console.WriteLine($"  8.5. Первый успешный выстрел: в клетку ({firstHit.Position.X}, {firstHit.Position.Y})");
            }
            else
            {
                Console.WriteLine("  8.5. Первый успешный выстрел: пока нет попаданий");
            }

            // 8.6. Список координат всех попаданий
            var hitCoordinates = boardShots
                .Where(s => s.Ship != null)
                .Select(s => $"({s.Position.X}, {s.Position.Y})");
            
            Console.WriteLine($"  8.6. Координаты попаданий: {(hitCoordinates.Any() ? string.Join(", ", hitCoordinates) : "нет")}");

            // 8.7.* Подсчет попаданий для каждого корабля и статус "потонул"
            // Поскольку у нас на доске пока один корабль, мы работаем с ним в цикле
            List<Ship> shipsOnBoard = new List<Ship> { board.Ship }; 
            Console.WriteLine("  8.7. Состояние кораблей на доске:");

            foreach (var ship in shipsOnBoard)
            {
                // Считаем уникальные попадания именно в этот корабль по истории выстрелов
                int shipHitsCount = boardShots
                    .Where(s => s.Ship == ship)
                    .Select(s => s.Position)
                    .Distinct() // Исключаем повторы, хотя повторный выстрел и так запрещен
                    .Count();

                bool isSunk = shipHitsCount >= ship.Length;
                Console.WriteLine($"       - Корабль (Длина: {ship.Length}): Попаданий: {shipHitsCount}/{ship.Length} | Статус: {(isSunk ? "ПОТОПЛЕН" : "На плаву")}");
            }
        }
        Console.WriteLine("--------------------------------");
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