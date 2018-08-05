using System;

class GuiController
{
    private MenuWindow _menuWindow;
    private CreditWindow _creditWindow;
    private ExitWindow _exitWindow;
    private GameOverWindow _gameOverWindow;
    private HighScoreWindow _highScoreWindow;

    public GuiController()
    {
        _menuWindow = new MenuWindow();
        _creditWindow = new CreditWindow();
        _exitWindow = new ExitWindow();
        _gameOverWindow = new GameOverWindow();
        _highScoreWindow = new HighScoreWindow();
    }

    public void ShowMenu()
    {
        Console.SetWindowSize(GameEngine.GameScreenWidth, GameEngine.GameScreenHeight);
        Console.SetBufferSize(GameEngine.GameScreenWidth, GameEngine.GameScreenHeight);

        RenderWindowIfNotAlreadyOpened(_menuWindow);

        int keyCode;

        do
        {
            keyCode = Console.ReadKey(true).Key.GetHashCode();
        } while (keyCode != 37 && keyCode != 39 && keyCode != 13);

        if (keyCode == 37)
        // left arrow
        {
            if (_menuWindow.GetCurrentButtonActive() > 0)
            {
                _menuWindow.SetButtonActive(_menuWindow.GetCurrentButtonActive() - 1);
                _menuWindow.RenderButtons();
            }
            ShowMenu();
        }

        else if (keyCode == 39)
        // right arrow
        {
            if (_menuWindow.GetCurrentButtonActive() < _menuWindow.ButtonCount - 1)
            {
                _menuWindow.SetButtonActive(_menuWindow.GetCurrentButtonActive() + 1);
                _menuWindow.RenderButtons();
            }
            ShowMenu();
        }

        else if (keyCode == 13)
        // enter
        {
            _menuWindow.IsAlreadyOpen = false;

            switch (_menuWindow.GetCurrentButtonActive())
            {
                case 0:
                    ShowCredits();
                    break;
                case 1:
                    ShowHighScore();
                    break;
                case 2:
                    // start game
                    GameEngine.StartGame();
                    break;
                case 3:
                    // enable/disable sound
                    SoundManager.EnableOrDisableMusic();
                    _menuWindow.IsAlreadyOpen = true;
                    ShowMenu();
                    break;
                case 4:
                    ShowExitWindow();
                    break;
            }
        }
    }

    public void ShowCredits()
    {
        RenderWindowIfNotAlreadyOpened(_creditWindow);

        int keyCode;

        do
        {
            keyCode = Console.ReadKey(true).Key.GetHashCode();
        } while (keyCode != 13 && keyCode != 27);

        if (keyCode == 13 || keyCode == 27)         // enter or ESC
        {
            _creditWindow.IsAlreadyOpen = false;
            ShowMenu();
        }
    }

    public void ShowExitWindow()
    {
        RenderWindowIfNotAlreadyOpened(_exitWindow);

        int keyCode;

        do
        {
            keyCode = Console.ReadKey(true).Key.GetHashCode();
        } while (keyCode != 37 && keyCode != 39 && keyCode != 13 && keyCode != 27);

        if (keyCode == 37)
        // left arrow
        {
            if (_exitWindow.GetCurrentButtonActive() > 0)
            {
                _exitWindow.SetButtonActive(_exitWindow.GetCurrentButtonActive() - 1);
                _exitWindow.RenderButtons();
            }
            ShowExitWindow();
        }

        else if (keyCode == 39)
        // right arrow
        {
            if (_exitWindow.GetCurrentButtonActive() < _exitWindow.ButtonCount - 1)
            {
                _exitWindow.SetButtonActive(_exitWindow.GetCurrentButtonActive() + 1);
                _exitWindow.RenderButtons();
            }
            ShowExitWindow();
        }

        else if (keyCode == 27)
        {
            _exitWindow.IsAlreadyOpen = false;
            ShowMenu();
        }

        else if (keyCode == 13)
        // enter
        {
            _exitWindow.IsAlreadyOpen = false;

            switch (_exitWindow.GetCurrentButtonActive())
            {
                case 0:
                    // exit game
                    Environment.Exit(0);
                    break;
                case 1:
                    ShowMenu();
                    SoundManager.LoadMusic(@"\Music\M.O.O.N. - 'Hydrogen'.wav");
                    break;
            }
        }

    }

    public void ShowGameOverWindow()
    {
        Console.SetWindowSize(GameEngine.GameScreenWidth, GameEngine.GameScreenHeight);
        Console.SetBufferSize(GameEngine.GameScreenWidth, GameEngine.GameScreenHeight);

        RenderWindowIfNotAlreadyOpened(_gameOverWindow);

        int keyCode;

        do
        {
            keyCode = Console.ReadKey(true).Key.GetHashCode();
        } while (keyCode != 37 && keyCode != 39 && keyCode != 13 && keyCode != 27);

        if (keyCode == 37)
        // left arrow
        {
            if (_gameOverWindow.GetCurrentButtonActive() > 0)
            {
                _gameOverWindow.SetButtonActive(_gameOverWindow.GetCurrentButtonActive() - 1);
                _gameOverWindow.RenderButtons();
            }
            ShowGameOverWindow();
        }

        else if (keyCode == 39)
        // right arrow
        {
            if (_gameOverWindow.GetCurrentButtonActive() < _gameOverWindow.ButtonCount - 1)
            {
                _gameOverWindow.SetButtonActive(_gameOverWindow.GetCurrentButtonActive() + 1);
                _gameOverWindow.RenderButtons();
            }
            ShowGameOverWindow();
        }

        else if (keyCode == 27)
        {
            // exit game
            Environment.Exit(0);
        }

        else if (keyCode == 13)
        // enter
        {
            _gameOverWindow.IsAlreadyOpen = false;

            switch (_gameOverWindow.GetCurrentButtonActive())
            {
                case 0:
                    // start game
                    GameEngine.StartGame();
                    break;
                case 1:
                    // open high score
                    ShowHighScore();
                    break;
                case 2:
                    // exit game
                    Environment.Exit(0);
                    break;
            }
        }
    }

    public void ShowHighScore()
    {
        Console.SetWindowSize(GameEngine.GameScreenWidth, GameEngine.GameScreenHeight);
        Console.SetBufferSize(GameEngine.GameScreenWidth, GameEngine.GameScreenHeight);

        RenderWindowIfNotAlreadyOpened(_highScoreWindow);

        int keyCode;

        do
        {
            keyCode = Console.ReadKey(true).Key.GetHashCode();
        } while (keyCode != 37 && keyCode != 39 && keyCode != 13 && keyCode != 27);

        if (keyCode == 37)
        // left arrow
        {
            if (_highScoreWindow.GetCurrentButtonActive() > 0)
            {
                _highScoreWindow.SetButtonActive(_highScoreWindow.GetCurrentButtonActive() - 1);
                _highScoreWindow.RenderButtons();
            }
            ShowHighScore();
        }

        else if (keyCode == 39)
        // right arrow
        {
            if (_highScoreWindow.GetCurrentButtonActive() < _highScoreWindow.ButtonCount - 1)
            {
                _highScoreWindow.SetButtonActive(_highScoreWindow.GetCurrentButtonActive() + 1);
                _highScoreWindow.RenderButtons();
            }
            ShowHighScore();
        }

        else if (keyCode == 27)
        {
            // show menu
            _highScoreWindow.IsAlreadyOpen = false;
            ShowMenu();
        }

        else if (keyCode == 13)
        // enter
        {
            _highScoreWindow.IsAlreadyOpen = false;

            switch (_highScoreWindow.GetCurrentButtonActive())
            {
                case 0:
                    // show menu
                    ShowMenu();
                    break;
                case 1:
                    // exit game
                    Environment.Exit(0);
                    break;
            }
        }

    }

    public static void ShowGameOverDialog()
    {
        GuiController gui = new GuiController();
        gui.ShowGameOverWindow();
    }

    private void RenderWindowIfNotAlreadyOpened(Window window)
    {
        if (!window.IsAlreadyOpen)
        {
            window.Render();
            window.IsAlreadyOpen = true;
        }
    }
}
