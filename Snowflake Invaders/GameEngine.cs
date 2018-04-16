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

        // sukuriam unit manager ir zaidimo langa
        unitManager = new GameFieldManager();
        gameField = new GameWindow(GameScreenWidth, GameScreenHeight);

        // reset previous starting score
        Score = 0;

        // sukuriam Hero ir jo bullet list
        unitManager.SetHero(new Hero(new UnitCoord(GameScreenWidth / 2, GameScreenHeight - 1)));

        // sukuriam enemies
        unitManager.CreateInitialSnowflakes();

        // zaidimas renderinamas
        Console.Clear();
        gameField.RenderGameBackground();
        unitManager.RenderUnits();
        needToRender = true;
        Console.CursorVisible = false;

        while (needToRender)
        {
            // Nustatomas zaidimo lygis ir greitis
            Level = GetLevelAndGameSpeed(Score, out gameSpeed);

            // Pajudinamas herojus
            ControlUnit();

            // Renderinama
            unitManager.RenderUnits();
            gameField.RenderScoreInfo();
            Thread.Sleep(gameSpeed / 2);

            // Pajudinami priesai
            unitManager.MoveAllBulletsUp();
            unitManager.MoveAllEnemiesDown();

            // Enemies uz ekrano ribu trinami, pridedami nauji enemies
            unitManager.MeltSnowflakes();
            unitManager.RemoveBullets();
            unitManager.AddBosses();

            // Renderinama
            unitManager.RenderUnits();
            gameField.RenderScoreInfo();
            Thread.Sleep(gameSpeed / 2);

            // Tikrina ar hero susiduria su enemies:
            IfGameOver();
        }
    }
    private static string GetLevelAndGameSpeed(int score, out int gameSpeed)
    {
        if (score < 100)
        {
            gameSpeed = 100;
            return "Summer in Canada";
        }
        else if (score < 200)
        {
            gameSpeed = 90;
            return "Diamond dust";
        }
        else if (score < 300)
        {
            gameSpeed = 80;
            return "Snow flurry";
        }
        else if (score < 400)
        {
            gameSpeed = 70;
            return "Snowsquall";
        }
        else if (score < 600)
        {
            gameSpeed = 60;
            return "Snow storm";
        }
        else if (score < 900)
        {
            gameSpeed = 50;
            return "Blizzard";
        }
        else if (score < 1200)
        {
            gameSpeed = 40;
            return "Arctic mayhem!";
        }
        else if (score < 1500)
        {
            gameSpeed = 25;
            return "THUNDER SNOW!";
        }
        else
        {
            gameSpeed = 10;
            return "»»» ULTRA INSTINCT «««";
        }
    }

    private static void OpenHighScoreManager()
    {
        // irasomas high score
        HighScoreManager hsm = new HighScoreManager();
        hsm.SetHighScore();
    }

    private static void IfGameOver()
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
