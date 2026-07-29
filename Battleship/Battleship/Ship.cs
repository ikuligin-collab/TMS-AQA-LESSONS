namespace Battleship;

public abstract class Ship
{
    // Координаты самой левой верней палубы
    public Position Position { get; }
    public int Length { get; }
    
    // Список выстрелов, которые попали в этот корабль
    public List<Shot> Hits { get; } = new List<Shot>();

    // Корабль потоплен, если количество попаданий равно его длине
    public bool IsSunk => Hits.Count >= Length;

    public Ship(Position position, int length)
    {
        // Проверка параметров корабля на отрицательную позицию
        if (length <= 0)
            throw new ArgumentException("Длина корабля должна быть больше 0!");
        Position = position;
        Length = length;
    }

    public abstract bool IsOnPosition(Position position);
    // Абстрактный метод проверки пересечения кораблей
    public abstract bool IntersectsWith(Ship other);

    // Вспомогательный метод для перебора всех координат палуб корабля
    public IEnumerable<Position> GetPositions()
    {
        for (int i = 0; i < Length; i++)
        {
            yield return this is HorizontalShip 
                ? new Position(Position.X + i, Position.Y)
                : new Position(Position.X, Position.Y + i);
        }
    }
}