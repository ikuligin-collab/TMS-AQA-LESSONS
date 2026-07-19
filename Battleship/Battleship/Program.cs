namespace Battleship;

class Program
{
    public static void Main()
    {
        try
        {
            // Настройки поля (например, 6х6 для демонстрации)
            var settings = new GameSettings(6, 6, PlayerType.Human);
            
            // Расставляем корабли игрока
            var humanShips = new Ship[] 
            { 
                new HorizontalShip(new Position(0, 0), 3), 
                new VerticalShip(new Position(4, 2), 2),
                new HorizontalShip(new Position(1, 4), 2)
            };

            var userBoard = new Board(settings.BoardRows, settings.BoardColumns, humanShips);
        
            // Передаем настройки прямо в конструктор (пункт 3)
            Game game = new Game(settings);

            // Начинаем партию
            game.Play(userBoard);
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Критическая ошибка: {ex.Message}");
        }
        finally
        {
            Console.WriteLine("\nНажмите любую клавишу для выхода...");
            Console.ReadKey();
        }
    }
}

// --- ВОТ ЭТОГО КУСКА НЕ ХВАТАЛО: ---

public record GameSettings(int BoardRows, int BoardColumns, PlayerType PlayerType)
{
    public GameSettings() : this(5, 5, PlayerType.Human) { }
}

public enum PlayerType { Human, Computer }