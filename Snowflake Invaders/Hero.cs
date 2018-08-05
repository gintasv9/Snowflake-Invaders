using System;
using System.Collections.Generic;

class Hero
{
    // └▓┘
    // ╔╬╗
    // ╔▓╗

    private char[] _heroChars = new char[] { '▓', '╔', '╬', '╗', '╔', '▓', '╗', '└', '┘' };

    private List<HeroChar> _completeHero = new List<HeroChar>();
    private List<UnitCoord> _heroCoordList = new List<UnitCoord>();

    private UnitCoord _centerCoord = new UnitCoord();

    public ConsoleColor Color { get; set; }

    public Hero(UnitCoord centerCoord)
    {
        _centerCoord.X = centerCoord.X;
        _centerCoord.Y = centerCoord.Y;

        _heroCoordList.Add(new UnitCoord(_centerCoord.X, _centerCoord.Y - 1));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X - 1, _centerCoord.Y));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X, _centerCoord.Y));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X + 1, _centerCoord.Y));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X - 1, _centerCoord.Y + 1));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X, _centerCoord.Y + 1));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X + 1, _centerCoord.Y + 1));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X - 1, _centerCoord.Y - 1));
        _heroCoordList.Add(new UnitCoord(_centerCoord.X + 1, _centerCoord.Y - 1));

        for (int i = 0; i < _heroCoordList.Count; i++)
        {
            _completeHero.Add(new HeroChar(_heroCoordList[i], _heroChars[i]));
        }

        Color = ConsoleColor.Cyan;
    }

    public void MoveRight()
    {
        ++_centerCoord.X;
        foreach (HeroChar unit in _completeHero)
        {
            unit.MoveRight();
        }
    }

    public void MoveLeft()
    {
        --_centerCoord.X;
        foreach (HeroChar unit in _completeHero)
        {
            unit.MoveLeft();
        }
    }

    public void MoveUp()
    {
        --_centerCoord.Y;
        foreach (HeroChar unit in _completeHero)
        {
            unit.MoveUp();
        }
    }

    public void MoveDown()
    {
        ++_centerCoord.Y;
        foreach (HeroChar unit in _completeHero)
        {
            unit.MoveDown();
        }
    }

    public int GetX()
    {
        return _centerCoord.X;
    }

    public int GetY()
    {
        return _centerCoord.Y;
    }

    public List<UnitCoord> GetCoordList()
    {
        List<UnitCoord> coordList = new List<UnitCoord>();
        foreach (var heroChar in _completeHero)
        {
            coordList.Add(new UnitCoord(heroChar.GetX(), heroChar.GetY()));
        }
        return coordList;
    }

    public List<HeroChar> GetHeroCharList()
    {
        return _completeHero;
    }

    public void PrintToScreen()
    {
        Console.ForegroundColor = Color;
        foreach (HeroChar unit in _completeHero)
        {
            unit.PrintToScreen();
        }
        Console.ResetColor();
    }
}
