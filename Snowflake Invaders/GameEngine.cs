using System;
using System.Threading;

static class GameEngine
{
    private static int gameSpeed;
    private static bool needToRender;

    private static GameWindow gameField;
    private static GameFieldManager unitManager;
    private static DateTime timeHeroWasMoved;

    public static int GameScreenHeight { get; } = 25;
    public static int GameScreenWidth { get; } = 100;
    public static int Score { get; set; }
    public static int FinalScore { get; private set; }
    public static string Level { get; private set; }

    public static void StartGame()
    {
        SoundManager.LoadMusic(@"\Music\vikce - casse-tête.wav");

        // Creating unit manager and game window
        unitManager = new GameFieldManager();
        gameField = new GameWindow(GameScreenWidth, GameScreenHeight);

        // Reset previous score
        Score = 0;

        // Set hero and bullets
        unitManager.SetHero(new Hero(new UnitCoord(GameScreenWidth / 2, GameScreenHeight - 1)));

        // Set enemies
        unitManager.CreateInitialSnowflakes();

        // Render game window
        Console.Clear();
        gameField.RenderGameBackground();
        unitManager.RenderUnits();
        needToRender = true;
        Console.CursorVisible = false;

        while (needToRender)
        {
            Level = GetLevelAndGameSpeed(Score, out gameSpeed);

            // Get hero control input
            ControlUnit();

            // Render
            unitManager.RenderUnits();
            gameField.RenderScoreInfo();
            Thread.Sleep(gameSpeed / 2);

            // Move enemies
            unitManager.MoveAllBulletsUp();
            unitManager.MoveAllEnemiesDown();

            // Cycle enemies
            unitManager.MeltSnowflakes();
            unitManager.RemoveBullets();
            unitManager.AddBosses();

            // Render
            unitManager.RenderUnits();
            gameField.RenderScoreInfo();
            Thread.Sleep(gameSpeed / 2);

            IsGameOver();
        }
    }

    public static string GetLevel(LevelEnum level)
    {
        switch (level)
        {
            case LevelEnum.start:
                return "Summer in Canada";
            case LevelEnum.first:
                return "Diamond dust";
            case LevelEnum.second:
                return "Snow flurry";
            case LevelEnum.third:
                return "Snowsquall";
            case LevelEnum.fourth:
                return "Snow storm";
            case LevelEnum.fifth:
                return "Blizzard";
            case LevelEnum.sixth:
                return "Arctic mayhem!";
            case LevelEnum.seventh:
                return "THUNDER SNOW!";
            case LevelEnum.final:
                return "»»» ULTRA INSTINCT «««";
            default:
                return "";
        }
    }

    private static string GetLevelAndGameSpeed(int score, out int gameSpeed)
    {
        if (score < 100)
        {
            gameSpeed = 100;
            return GetLevel(LevelEnum.start);
        }
        else if (score < 200)
        {
            gameSpeed = 90;
            return GetLevel(LevelEnum.first);
        }
        else if (score < 300)
        {
            gameSpeed = 80;
            return GetLevel(LevelEnum.second);
        }
        else if (score < 400)
        {
            gameSpeed = 70;
            return GetLevel(LevelEnum.third);
        }
        else if (score < 600)
        {
            gameSpeed = 60;
            return GetLevel(LevelEnum.fourth);
        }
        else if (score < 900)
        {
            gameSpeed = 50;
            return GetLevel(LevelEnum.fifth);
        }
        else if (score < 1200)
        {
            gameSpeed = 40;
            return GetLevel(LevelEnum.sixth);
        }
        else if (score < 1500)
        {
            gameSpeed = 25;
            return GetLevel(LevelEnum.seventh);
        }
        else
        {
            gameSpeed = 10;
            return GetLevel(LevelEnum.final);
        }
    }

    private static void OpenHighScoreManager()
    {
        HighScoreManager hsm = new HighScoreManager();
        hsm.SetHighScore();
    }

    private static void IsGameOver()
    {
        if (unitManager.CheckForEndgame())
        {
            needToRender = false;
            unitManager.RenderUnits();
            Thread.Sleep(2000);

            SoundManager.LoadMusic(@"\Music\MOON - Dust.wav");

            FinalScore = Score;
            OpenHighScoreManager();
            GuiController.ShowGameOverDialog();
        }
    }

    private static void ControlUnit()
    {
        timeHeroWasMoved = DateTime.Now;

        while (Console.KeyAvailable)
        {
            ConsoleKeyInfo pressedKey = Console.ReadKey(true);

            if (pressedKey.Key == ConsoleKey.Escape)
            {
                SoundManager.LoadMusic(@"\Music\MOON - Dust.wav");

                needToRender = false;
                FinalScore = Score;
                OpenHighScoreManager();
                GuiController.ShowGameOverDialog();
                break;
            }
            if (pressedKey.Key == ConsoleKey.Spacebar)
            {
                unitManager.AddDoubleBullet(new Bullet(new UnitCoord(unitManager.GetHero().GetX() - 1, unitManager.GetHero().GetY() - 1)),
                                            new Bullet(new UnitCoord(unitManager.GetHero().GetX() + 1, unitManager.GetHero().GetY() - 1)));
            }
            if (pressedKey.Key == ConsoleKey.LeftArrow)
            {
                if (gameField.CanUnitMoveLeft(unitManager.GetHero().GetX() - 1))
                {
                    unitManager.GetHero().MoveLeft();
                }
            }
            if (pressedKey.Key == ConsoleKey.RightArrow)
            {
                if (gameField.CanUnitMoveRight(unitManager.GetHero().GetX() + 1))
                {
                    unitManager.GetHero().MoveRight();
                }
            }
            if (pressedKey.Key == ConsoleKey.UpArrow)
            {
                if (gameField.CanUnitMoveUp(unitManager.GetHero().GetY() - 1))
                {
                    unitManager.GetHero().MoveUp();
                }
            }
            if (pressedKey.Key == ConsoleKey.DownArrow)
            {
                if (gameField.CanUnitMoveDown(unitManager.GetHero().GetY() + 1))
                {
                    unitManager.GetHero().MoveDown();
                }
            }
        }

    }

}
