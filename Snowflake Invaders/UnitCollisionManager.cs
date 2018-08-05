using System;
using System.Collections.Generic;

class UnitCollisionManager
{
    public bool CheckForCollision(List<IEnemy> enemyList, Hero hero)
    {
        List<UnitCoord> enemyCoordList = new List<UnitCoord>();
        List<UnitCoord> heroCoordList = new List<UnitCoord>();

        heroCoordList = hero.GetCoordList();

        foreach (var enemy in enemyList)
        {
            enemyCoordList = enemy.GetCoordList();

            foreach (var enemyCoord in enemyCoordList)
            {
                foreach (var heroCoord in heroCoordList)
                {
                    if (heroCoord.X == enemyCoord.X && heroCoord.Y == enemyCoord.Y)
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public bool IsEnemyShot(IEnemy enemy, List<Bullet> bulletList)
    {
        List<UnitCoord> enemyCoordList = new List<UnitCoord>();
        enemyCoordList = enemy.GetCoordList();

        foreach (var enemyCoord in enemyCoordList)
        {
            foreach (var bullet in bulletList)
            {
                // || (bullet.GetY() - 1 == enemyCoord.Y) is needed, so that snowflakes and bullets would not pass each other through
                if (bullet.GetX() == enemyCoord.X && (bullet.GetY() == enemyCoord.Y || bullet.GetY() - 1 == enemyCoord.Y))
                {
                    bulletList.Remove(bullet);
                    if (SoundManager.IsSoundEnabled)
                    {
                    Console.Beep(500, 30);
                    }
                    return true;
                }
            }
        }
        return false;
    }
}
