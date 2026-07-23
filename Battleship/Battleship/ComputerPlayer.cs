namespace Battleship;

class ComputerPlayer : IPlayer
{
    public Shot Shoot(Board targetBoard)
    {
        var random = new Random();
        // Компьютер просто выбирает случайную позицию в пределах доски
        var shotPosition = targetBoard.GeneratePosition(random);
        
        Ship? targetShip = targetBoard.Ships.FirstOrDefault(s => s.IsOnPosition(shotPosition));
        return new Shot(targetBoard, shotPosition, targetShip);
    }

    public void WriteName()
    {
        Console.WriteLine("Компьютер (Робот)");
    }
}