using System;
using System.Collections.Generic;

class GameOverWindow : Window
{
    private readonly TextBlock _gameOverText;
    private readonly TextLine _yourScore;
    private List<Button> _availableButtons = new List<Button>();
    private int _currentButtonActive;

    public int ButtonCount { get; private set; }

    public GameOverWindow() : base(0, 0, GameEngine.GameScreenHeight, GameEngine.GameScreenWidth, '█')
    {
        List<string> gameOverMsg = new List<string>
        {
            @"  _______      ___      .___  ___.  _______      ______   ____    ____  _______ .______      ",
            @" /  _____|    /   \     |   \/   | |   ____|    /  __  \  \   \  /   / |   ____||   _  \     ",
            @"|  |  __     /  ^  \    |  \  /  | |  |__      |  |  |  |  \   \/   /  |  |__   |  |_)  |    ",
            @"|  | |_ |   /  /_\  \   |  |\/|  | |   __|     |  |  |  |   \      /   |   __|  |      /     ",
            @"|  |__| |  /  _____  \  |  |  |  | |  |____    |  `--'  |    \    /    |  |____ |  |\  \----.",
            @" \______| /__/     \__\ |__|  |__| |_______|    \______/      \__/     |_______|| _| `._____|"
        };

        _gameOverText = new TextBlock(1, 3, 100, gameOverMsg);
        _yourScore = new TextLine((100 - 20) / 2, 13, 20, $"Your score: {GameEngine.FinalScore}");

        _availableButtons.Add(new Button(8, 15, 7, 20, "Start again!"));
        _availableButtons.Add(new Button(40, 15, 7, 20, "High score"));
        _availableButtons.Add(new Button(72, 15, 7, 20, "Exit game"));

        ButtonCount = _availableButtons.Count;

        _currentButtonActive = 0;
        _availableButtons[_currentButtonActive].SetActive();
    }

    public override void Render()
    {
        //Console.Clear();

        base.Render();
        _gameOverText.Render();
        _yourScore.Render();

        RenderButtons();

        Console.SetCursorPosition(0, 0);
    }

    public void RenderButtons()
    {
        for (int i = 0; i < ButtonCount; i++)
        {
            _availableButtons[i].Render();
        }
    }


    public void SetButtonActive(int activeButtonIndex)
    {
        _availableButtons[_currentButtonActive].SetInactive();
        _currentButtonActive = activeButtonIndex;
        _availableButtons[_currentButtonActive].SetActive();
    }

    public int GetCurrentButtonActive()
    {
        return _currentButtonActive;
    }
}
