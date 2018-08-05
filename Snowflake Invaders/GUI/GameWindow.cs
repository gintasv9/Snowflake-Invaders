using System;
using System.Collections.Generic;

class GameWindow
{
    private int _screenWidth;
    private int _screenHeight;

    private TextLine _bottomBorder;
    private TextBlock _scoreInfo;
    private Frame _gameFrame;
    private Frame _scoreFrame;

    private List<string> _scoreList;
    private Random _rng = new Random();

    public GameWindow(int width, int height)
    {
        _screenWidth = width;
        _screenHeight = height;

        _bottomBorder = new TextLine(2, height + 1, width - 2, new string('*', width - 3));
        _gameFrame = new Frame(0, 0, height + 3, width + 1, '▓');

        _scoreFrame = new Frame(0, height + 4, 9, width + 1, '█');

        Console.SetWindowSize(_screenWidth + 1, _screenHeight + 13);
        Console.SetBufferSize(_screenWidth + 1, _screenHeight + 13);

        SafeNativeMethods.CenterConsole();
    }

    public bool CanUnitMoveLeft(int x)
    {
        if (x > 2)
        {
            return true;
        }
        return false;
    }

    public bool CanUnitMoveRight(int x)
    {
        if (x < _screenWidth - 2)
        {
            return true;
        }
        return false;
    }

    public bool CanUnitMoveDown(int y)
    {
        if (y < _screenHeight)
        {
            return true;
        }
        return false;
    }

    public bool CanUnitMoveUp(int y)
    {
        if (y >= 3)
        {
            return true;
        }
        return false;
    }

    public void RenderGameBackground()
    {
        _gameFrame.Render();
        _scoreFrame.Render();
        Console.ResetColor();
        _bottomBorder.Render();
    }

    public void RenderScoreInfo()
    {
        // whiteSpace - kad nereiktu trinti pries tai buvusio Score ir Level teksto
        string whiteSpace = new string(' ', _screenWidth / 4);

        _scoreList = new List<string> { $"{whiteSpace}SCORE: {GameEngine.Score}{whiteSpace}", "", $"{whiteSpace}LEVEL: {GameEngine.Level}{whiteSpace}" };
        _scoreInfo = new TextBlock(2, _screenHeight + 7, _screenWidth, _scoreList);
        _scoreInfo.Render(ConsoleColor.Blue);
    }
}
