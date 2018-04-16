using System;
using System.Collections.Generic;

class Snowflake : Unit, IEnemy
{
    private int _id;

    public Snowflake(int id, UnitCoord coord) : base(coord, '■')
    {
        _id = id;
    }

    public int MoveDown()
    {
        if (_coord.Y > GameEngine.GameScreenHeight + 2)
        {
            throw new Exception("Snowflake out of bounds!");
        }
        return ++_coord.Y;
    }

    public int GetId()
    {
        return _id;
    }

    public List<UnitCoord> GetCoordList()
    {
        return new List<UnitCoord>() { _coord };
    }

    public override void PrintToScreen()
    {
        Console.ForegroundColor = ConsoleColor.White;
        Console.SetCursorPosition(_coord.X, _coord.Y);
        Console.Write(Symbol);
        Console.ResetColor();
    }
}
