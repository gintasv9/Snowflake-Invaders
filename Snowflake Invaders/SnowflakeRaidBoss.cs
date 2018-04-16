using System;
using System.Collections.Generic;

class SnowflakeRaidBoss : IEnemy
{
    private List<Snowflake> _boss = new List<Snowflake>();
    private List<UnitCoord> _coordList = new List<UnitCoord>();

    private Random _rng = new Random();
    private const int BOSS_ID = -2;
    private const int SYMBOL_COUNT = 17;

    public int Y { get; private set; }

    public SnowflakeRaidBoss()
    {
        int bossXCoord = _rng.Next(2, GameEngine.GameScreenWidth - 7);
        int bossYCoord = 2;

        // pridedamos (17 - 1) snaigiu suformuoja "*" forma su spinduliais
        //  *   *
        //   ***
        // *** ***
        //   ***
        //  *   *
        for (int i = 0; i < SYMBOL_COUNT; i++)
        {
            if (i < 3)
            {
                ;
                _coordList.Add(new UnitCoord(bossXCoord + 1 + i, bossYCoord));
            }
            else if (i < 10 && i != 6)
            {
                _coordList.Add(new UnitCoord(bossXCoord + (i - 3 - 1), bossYCoord + 1));
            }
            else if (i < 13 && i != 6)
            {
                _coordList.Add(new UnitCoord(bossXCoord + 1 + (i - 10), bossYCoord + 2));
            }
            else if (i == 13)
            {
                _coordList.Add(new UnitCoord(bossXCoord, bossYCoord - 1));
            }
            else if (i == 14)
            {
                _coordList.Add(new UnitCoord(bossXCoord + 4, bossYCoord - 1));
            }
            else if (i == 15)
            {
                _coordList.Add(new UnitCoord(bossXCoord, bossYCoord + 3));
            }
            else if (i != 6)
            {
                _coordList.Add(new UnitCoord(bossXCoord + 4, bossYCoord + 3));
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

        // Y - snaiges centro Y koordinate
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

