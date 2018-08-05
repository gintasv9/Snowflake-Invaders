using System;

class Bullet : Unit
{
    public DateTime CreationTime { get; private set; }

    public Bullet(UnitCoord coord) : base(coord, '▲')   // possible symbols: '¤', '×'
    {
        CreationTime = DateTime.Now;
    }

    public override void PrintToScreen()
    {
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        base.PrintToScreen();
        Console.ResetColor();
    }

    public int MoveUp()
    {
        return --_coord.Y;
    }
}
