using System;

abstract class Unit
{
    protected UnitCoord _coord;
    public char Symbol { get; set; }

    public Unit(UnitCoord coord, char symbol)
    {
        _coord.X = coord.X;
        _coord.Y = coord.Y;
        Symbol = symbol;
    }

    public virtual int GetX()
    {
        return _coord.X;
    }

    public virtual int GetY()
    {
        return _coord.Y;
    }

    public virtual void PrintToScreen()
    {
        Console.SetCursorPosition(_coord.X, _coord.Y);
        Console.Write(Symbol);
    }
}
