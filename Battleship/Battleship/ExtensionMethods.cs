namespace Battleship;

static class ExtensionMethods
{
    public static Position GeneratePosition(this Board targetBoard, Random random)
    {
        return new Position(random.Next(0, targetBoard.Columns), random.Next(0, targetBoard.Rows));
    }

    // Метод генерации случайного корабля (пункт 3)
    public static Ship GenerateRandomShip(this Random random, GameSettings settings, int length)
    {
        bool isHorizontal = random.Next(0, 2) == 0;
        
        // Ограничиваем начальные координаты, чтобы корабль гарантирован залез на доску
        int maxX = isHorizontal ? settings.BoardColumns - length : settings.BoardColumns - 1;
        int maxY = isHorizontal ? settings.BoardRows - 1 : settings.BoardRows - length;

        // На всякий случай страхуемся от некорректных настроек
        int x = random.Next(0, Math.Max(1, maxX + 1));
        int y = random.Next(0, Math.Max(1, maxY + 1));
        
        Position pos = new Position(x, y);

        return isHorizontal 
            ? new HorizontalShip(pos, length) 
            : new VerticalShip(pos, length);
    }
}