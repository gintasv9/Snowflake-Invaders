using System;
using System.Collections.Generic;
using System.Text;

class GameFieldRenderer
{
    private const int LOWER_GAME_FIELD_BUFFER = 4;
    private char[,] _emptyGameField = new char[GameEngine.GameScreenWidth, GameEngine.GameScreenHeight + LOWER_GAME_FIELD_BUFFER - 1];
    private char[,] _currentGameField = new char[GameEngine.GameScreenWidth, GameEngine.GameScreenHeight + LOWER_GAME_FIELD_BUFFER - 1];

    public GameFieldRenderer()
    {
        PopulateCharArrayWithWhitespace(_emptyGameField);
    }

    public void PopulateCharArrayWithWhitespace(char[,] array)
    {
        for (int i = 1; i <= array.GetLength(1) - 1; i++)
        {
            for (int j = 0; j < array.GetLength(0) - 1; j++)
            {
                array[j, i] = ' ';
            }
        }
    }

    public void SetupGameFieldArray(List<IEnemy> enemyList, List<Bullet> bulletList, Hero hero)
    {
        Array.Copy(_emptyGameField, _currentGameField, _emptyGameField.Length);

        foreach (var enemy in enemyList)
        {
            List<UnitCoord> enemyCoords = new List<UnitCoord>();
            enemyCoords = enemy.GetCoordList();

            foreach (var coord in enemyCoords)
            {
                // '■' - enemy simbolis, NOTE: padaryti scalable
                _currentGameField[coord.X, coord.Y] = '■';
            }
        }

        // NEREIKALINGAS KODAS - bullets ir Hero RenderUnits() nupiesia spalvotai.

        // foreach (var bullet in bulletList)
        // {
        //     List<UnitCoord> bulletCoords = new List<UnitCoord>();
        //     bulletCoords.Add(new UnitCoord(bullet.GetX(), bullet.GetY()));
           
        //     foreach (var coord in bulletCoords)
        //     {
        //         _currentGameField[coord.X, coord.Y] = (bullet as Unit).Symbol;
        //     }
        // }
           
        // List<HeroChar> heroCharList = hero.GetHeroCharList();
           
        // foreach (var heroChar in heroCharList)
        // {
        //     _currentGameField[heroChar.GetX(), heroChar.GetY()] = heroChar.Symbol;
        // }
    }      

    public void RenderGameField(ConsoleColor color)
    {
        Console.ForegroundColor = color;

        // row ir col prasideda 1, kad nerenderintu ant virsaus GameWindow frame'o!
        for (int row = 1; row <= _currentGameField.GetLength(1) - LOWER_GAME_FIELD_BUFFER + 1; row++)
        {
            StringBuilder sb = new StringBuilder();
            string gameField = "";

            for (int col = 1; col <= _currentGameField.GetLength(0) - 1; col++)
            {
                sb.Append(_currentGameField[col, row]);

                // Atsarginis planas: renderinamas kiekvienas simbolis
                // Console.SetCursorPosition(col, row);
                // Console.Write(_currentGameField[col, row]);
            }

            // Renderinama po viena eilute
            gameField = sb.ToString();
            Console.SetCursorPosition(1, row);
            Console.Write(gameField);
        }
        Console.ResetColor();
    }
}
