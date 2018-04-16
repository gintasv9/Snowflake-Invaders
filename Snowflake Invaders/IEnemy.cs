using System.Collections.Generic;

interface IEnemy
{
    int MoveDown();
    void PrintToScreen();
    List<UnitCoord> GetCoordList();
}
