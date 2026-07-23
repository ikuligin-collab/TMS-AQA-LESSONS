namespace Battleship;

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
