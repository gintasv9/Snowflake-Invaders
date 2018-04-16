using System;
using System.Collections.Generic;

class SnowflakeBoss : IEnemy
{
    private List<Snowflake> _boss = new List<Snowflake>();
    private List<UnitCoord> _coordList = new List<UnitCoord>();

    private Random _rng = new Random();
    private const int BOSS_ID = -1;
    private const int SYMBOL_COUNT = 11;

    public int Y { get; private set; }

    public SnowflakeBoss()
    {
        int bossXCoord = _rng.Next(1, GameEngine.GameScreenWidth - 5);
        int bossYCoord = _rng.Next(1, 3);

        // pridedamos (11 - 1) snaigiu suformuoja "*" forma
        //  ***
        // ** **
        //  ***
        for (int i = 0; i < SYMBOL_COUNT; i++)
        {
            if (i < 3)
            {
                _coordList.Add(new UnitCoord(bossXCoord + 1 + i, bossYCoord));
            }
            else if (i < 8 && i != 5)
            {
                _coordList.Add(new UnitCoord(bossXCoord + (i - 3), bossYCoord + 1));
            }
            else if (i < SYMBOL_COUNT && i != 5)
            {
                _coordList.Add(new UnitCoord(bossXCoord + 1 + (i - 8), bossYCoord + 2));
            }
        }

        for (int i = 0; i < _coordList.Count; i++)
        {
            _boss.Add(new Snowflake(BOSS_ID, _coordList[i]));
        }
    }

    public int MoveDown()
    {
        int sumOfYCoords = 0;

        foreach (Snowflake snowflake in _boss)
        {
            sumOfYCoords += snowflake.MoveDown();
        }

        Y = sumOfYCoords / (SYMBOL_COUNT - 1);
        return sumOfYCoords / (SYMBOL_COUNT - 1);
    }

    public List<UnitCoord> GetCoordList()
    {
        List<UnitCoord> coordList = new List<UnitCoord>();
        foreach (var snowflake in _boss)
        {
            coordList.Add(new UnitCoord(snowflake.GetX(), snowflake.GetY()));
        }
        return coordList;
    }

    public void PrintToScreen()
    {
        foreach (Snowflake snowflake in _boss)
        {
            snowflake.PrintToScreen();
        }
    }
}

