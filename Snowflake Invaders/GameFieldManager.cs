using System;
using System.Collections.Generic;

class GameFieldManager
{
    private Hero _hero;
    private List<IEnemy> _enemies = new List<IEnemy>();
    private List<Bullet> _bulletList = new List<Bullet>();
    private UnitCollisionManager _ucm = new UnitCollisionManager();

    private Random _rng = new Random();

    public int EnemyCount { get; private set; }

    public void SetHero(Hero hero)
    {
        _hero = hero;
    }

    public Hero GetHero()
    {
        return _hero;
    }

    public void AddEnemy(Snowflake enemy)
    {
        _enemies.Add(enemy);
    }

    public void AddBosses()
    {
        // tarpiniai BOSAI
        if (EnemyCount % 5 == 4 || EnemyCount % 5 == 3)
        {
            _enemies.Add(new SnowflakeBoss());
        }

        // raid BOSAI
        if (EnemyCount % 16 == 3)
        {
            _enemies.Add(new SnowflakeRaidBoss());
        }
    }

    public void MoveAllEnemiesDown()
    {
        foreach (IEnemy enemy in _enemies)
        {
            enemy.MoveDown();
        }
    }

    public Snowflake getEnemyById(int id)
    {
        foreach (Snowflake enemy in _enemies)
        {
            if (enemy.GetId() == id)
            {
                return enemy;
            }
        }
        // jeigu nera priesu?
        return null;
    }

    public void AddDoubleBullet(Bullet bullet1, Bullet bullet2)
    {
        if (_bulletList.Count > 0)
        {
            if ((bullet1.CreationTime - _bulletList[_bulletList.Count - 1].CreationTime).TotalMilliseconds > 750)
            {
                _bulletList.Add(bullet1);
                _bulletList.Add(bullet2);
                _hero.Color = (ConsoleColor)(_rng.Next(1, 14));
            }
        }
        else
        {
            _bulletList.Add(bullet1);
            _bulletList.Add(bullet2);
        }
    }

    public void MoveAllBulletsUp()
    {
        foreach (Bullet bullet in _bulletList)
        {
            if (bullet.GetY() >= 2)
            {
                bullet.MoveUp();
            }
        }
    }

    public void RemoveBullets()
    {
        for (int i = 0; i <= _bulletList.Count - 1; i++)
        {
            if (_bulletList[i].GetY() <= 1)
            {
                _bulletList.RemoveAt(i);
            }
        }
    }

    public void CreateInitialSnowflakes()
    {
        for (int i = 1; i <= GameEngine.GameScreenWidth / 6; i++)
        {
            EnemyCount++;
            int enemyXCoord = _rng.Next(1, GameEngine.GameScreenWidth);
            int enemyYCoord = _rng.Next(1, GameEngine.GameScreenHeight / 2);

            AddEnemy(new Snowflake(EnemyCount, new UnitCoord(enemyXCoord, enemyYCoord)));
        }
    }

    // snaiges trinamos kai pasiekia zeme arba kai susiduria su kulka
    public void MeltSnowflakes()
    {
        for (int i = 0; i < _enemies.Count; i++)
        {
            if (_enemies[i] is Snowflake)
            {
                if (((Snowflake)_enemies[i]).GetY() > GameEngine.GameScreenHeight || _ucm.IsEnemyShot(_enemies[i], _bulletList))
                {
                    _enemies.Remove(_enemies[i]);
                    // kai pasalinamas priesas, patikrinamas mazesnis indeksas
                    i--;

                    EnemyCount++;
                    GameEngine.Score++;
                    int enemyXCoord = _rng.Next(1, GameEngine.GameScreenWidth);
                    int enemyYCoord = _rng.Next(1, GameEngine.GameScreenHeight / 2);
                    AddEnemy(new Snowflake(EnemyCount, new UnitCoord(enemyXCoord, enemyYCoord)));
                }
            }
            else if (_enemies[i] is SnowflakeBoss)
            {
                if (((SnowflakeBoss)_enemies[i]).Y > GameEngine.GameScreenHeight)
                {
                    _enemies.Remove(_enemies[i]);
                    // kai pasalinamas priesas, patikrinamas mazesnis indeksas
                    i--;

                    GameEngine.Score += 2;
                }
                else if (_ucm.IsEnemyShot(_enemies[i], _bulletList))
                {
                    _enemies.Remove(_enemies[i]);
                    i--;

                    GameEngine.Score += 6;
                }
            }
            else
            {
                if (((SnowflakeRaidBoss)_enemies[i]).Y > GameEngine.GameScreenHeight - 1)
                {
                    _enemies.Remove(_enemies[i]);
                    // kai pasalinamas priesas, patikrinamas mazesnis indeksas
                    i--;

                    GameEngine.Score += 5;
                }
                else if (_ucm.IsEnemyShot(_enemies[i], _bulletList))
                {
                    _enemies.Remove(_enemies[i]);
                    i--;

                    GameEngine.Score += 15;
                }
            }
        }
    }

    public bool CheckForEndgame()
    {
        // patikrinamas collision
        return _ucm.CheckForCollision(_enemies, _hero);
    }

    public void RenderUnits()
    {
        // balta spalva, renderinamos snowflakes kas 1 eilte:
        GameFieldRenderer gfr = new GameFieldRenderer();
        gfr.SetupGameFieldArray(_enemies, _bulletList, _hero);
        gfr.RenderGameField(ConsoleColor.White);

        // spalvotai renderinami hero ir bullets:
        _hero.PrintToScreen();

        foreach (Bullet bullet in _bulletList)
        {
            bullet.PrintToScreen();
        }
    }

}
