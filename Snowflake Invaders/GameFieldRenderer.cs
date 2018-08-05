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
    }      

    public void RenderGameField(ConsoleColor color)
    {
        Console.ForegroundColor = color;

        // row ir col prasideda 2, kad nerenderintu ant virsaus GameWindow frame'o!
        for (int row = 1; row <= _currentGameField.GetLength(1) - LOWER_GAME_FIELD_BUFFER + 1; row++)
        {
            StringBuilder sb = new StringBuilder();
            string gameField = "";

            for (int col = 2; col <= _currentGameField.GetLength(0) - 2; col++)
            {
                sb.Append(_currentGameField[col, row]);
            }

            // Renderinama po viena eilute
            gameField = sb.ToString();
            Console.SetCursorPosition(2, row);
            Console.Write(gameField);
        }
        Console.ResetColor();
    }
}
