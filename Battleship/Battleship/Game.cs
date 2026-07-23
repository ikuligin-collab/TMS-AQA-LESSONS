namespace Battleship;

using System;
using System.Collections.Generic;
using System.Linq;

class Game
{
    private readonly GameSettings _settings;
    private readonly List<Shot> _shotsHistory = new List<Shot>();

    public Game(GameSettings settings)
    {
        _settings = settings;
    }

    public void Play(Board humanBoard)
    {
        // Создаем игроков через интерфейсы (пункт 1.3)
        IPlayer human = new HumanPlayer();
        IPlayer computer = new ComputerPlayer();

        // Генерируем зеркальную доску для компьютера (пункт 3)
        Board computerBoard = GenerateOpponentBoard(humanBoard);

        Console.Clear();
        Console.WriteLine("--- ИГРА НАЧАЛАСЬ ---");

        while (true)
        {
            // --- 1. ХОД ЧЕЛОВЕКА ---
            Console.WriteLine("\n--- ВАШ ХОД ---");
            ExecutePlayerTurn(human, computerBoard);

            // Проверка на окончание игры после хода человека
            if (CheckGameOver(computerBoard, "Поздравляем! Вы победили!")) break;

            // --- 2. ХОД КОМПЬЮТЕРА ---
            Console.WriteLine("\n--- ХОД КОМПЬЮТЕРА ---");
            ExecutePlayerTurn(computer, humanBoard);

            // Проверка на окончание игры после хода компьютера
            if (CheckGameOver(humanBoard, "Компьютер победил! В следующий раз повезет.")) break;

            // --- 3. ВЫВОД ИНФОРМАЦИИ В КОНЦЕ РАУНДА ---
            Console.WriteLine("\n================ ТЕКУЩЕЕ СОСТОЯНИЕ ================");
            Console.WriteLine("ДОСКА ИГРОКА:");
            DrawBoard(humanBoard, showHiddenShips: true);
            
            Console.WriteLine("\nДОСКА КОМПЬЮТЕРА:");
            DrawBoard(computerBoard, showHiddenShips: false);

            // Вывод количества потопленных кораблей через LINQ (пункт 5)
            int humanSunk = humanBoard.Ships.Count(s => s.IsSunk);
            int computerSunk = computerBoard.Ships.Count(s => s.IsSunk);

            Console.WriteLine($"\n[Статистика раунда] Потоплено ваших кораблей: {humanSunk}/{humanBoard.Ships.Length}");
            Console.WriteLine($"[Статистика раунда] Потоплено кораблей компьютера: {computerSunk}/{computerBoard.Ships.Length}");
            Console.WriteLine("====================================================");
            
            Console.WriteLine("Нажмите любую клавишу для перехода к следующему раунду...");
            Console.ReadKey();
            Console.Clear();
        }
    }

    private void ExecutePlayerTurn(IPlayer shooter, Board targetBoard)
    {
        while (true)
        {
            try
            {
                // Игрок делает выстрел
                Shot shot = shooter.Shoot(targetBoard);

                // Подсказка из ТЗ 1.5: Сначала проверяем на повторность
                bool isDuplicate = _shotsHistory.Any(s => s.Board == targetBoard && 
                                                          s.Position.X == shot.Position.X && 
                                                          s.Position.Y == shot.Position.Y);
                if (isDuplicate)
                {
                    // Если это бот, он просто тихо перевыберет клетку, если человек — кидаем ошибку
                    throw new InvalidOperationException("В эту клетку уже стреляли!");
                }

                // Если выстрел уникальный — записываем его в историю
                _shotsHistory.Add(shot);

                // Если попали — добавляем выстрел в коллекцию корабля (пункт 5)
                if (shot.Result == ShootResult.Hit && shot.Ship != null)
                {
                    shot.Ship.Hits.Add(shot);
                    Console.WriteLine($"Результат: ПОПАДАНИЕ по координатам ({shot.Position.X}, {shot.Position.Y})!");
                    if (shot.Ship.IsSunk)
                    {
                        Console.WriteLine("Корабль ПОТОПЛЕН!");
                    }
                }
                else
                {
                    Console.WriteLine($"Результат: Мимо по координатам ({shot.Position.X}, {shot.Position.Y}).");
                }

                break; // Выстрел успешен, выходим из цикла переповтора хода
            }
            catch (Exception ex)
            {
                // Выводим ошибку только для человека, чтобы не спамить консоль ошибками ИИ
                if (shooter is HumanPlayer)
                {
                    Console.WriteLine($"Ошибка хода: {ex.Message} Попробуйте еще раз.");
                }
            }
        }
    }

    private Board GenerateOpponentBoard(Board humanBoard)
    {
        Random rand = new Random();
        List<Ship> opponentShips = new List<Ship>();

        // Генерируем столько же кораблей, сколько и у человека, таких же длин
        foreach (var humanShip in humanBoard.Ships)
        {
            while (true)
            {
                Ship potentialShip = rand.GenerateRandomShip(_settings, humanShip.Length);

                // Проверяем на пересечение со всеми уже созданными (пункт 3)
                bool intersects = opponentShips.Any(s => s.IntersectsWith(potentialShip));

                if (!intersects)
                {
                    opponentShips.Add(potentialShip);
                    break; // Корабль успешно сел на карту
                }
            }
        }

        return new Board(_settings.BoardRows, _settings.BoardColumns, opponentShips.ToArray());
    }

    private void DrawBoard(Board board, bool showHiddenShips)
    {
        // 1. Выводим шапку с номерами столбцов (добавляем 3 пробела для компенсации номера строки и вертикальной черты)
        Console.Write("    ");
        for (int col = 0; col < board.Columns; col++) 
            Console.Write(col + " ");
        Console.WriteLine();

        // Разделительная линия для красоты
        Console.WriteLine("  +-" + string.Concat(Enumerable.Repeat("--", board.Columns)));

        // 2. Двойной цикл построения поля
        for (int row = 0; row < board.Rows; row++)
        {
            Console.Write(row + " | "); // Выводим номер строки и красивый разделитель

            for (int col = 0; col < board.Columns; col++)
            {
                var currentPos = new Position(col, row);

                // Ищем, был ли выстрел в эту клетку на этой конкретной доске
                var pastShot = _shotsHistory.FirstOrDefault(s => s.Board == board && 
                                                                 s.Position.X == col && 
                                                                 s.Position.Y == row);

                if (pastShot != null)
                {
                    Console.Write(pastShot.Result == ShootResult.Hit ? "X " : "O ");
                }
                else if (board.HasShip(currentPos) && showHiddenShips)
                {
                    Console.Write("S "); // Неповрежденная палуба игрока
                }
                else
                {
                    Console.Write(". "); // Пустая клетка
                }
            }
            Console.WriteLine();
        }
    }

    private bool CheckGameOver(Board board, string winMessage)
    {
        // Если все корабли на доске потоплены (пункт 5)
        if (board.Ships.All(s => s.IsSunk))
        {
            Console.WriteLine($"\nКОНЕЦ ИГРЫ! {winMessage}");
            return true;
        }
        return false;
    }
}