namespace Battleship;

abstract class Ship
{
    // Координаты самой левой верней палубы
    public Position Position { get; }
    public int Length { get; }

    public Ship(Position position, int length)
    {
        // Проверка параметров корабля на отрицательную позицию
        if (length <= 0)
            throw new ArgumentException("Длина корабля должна быть больше 0!");
        Position = position;
        Length = length;
    }

    public abstract bool IsOnPosition(Position position);
}