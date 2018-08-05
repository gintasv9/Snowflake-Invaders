using System;
using System.Collections.Generic;

class HighScoreWindow : Window
{
    private readonly TextBlock _highScoreText;
    private readonly TextBlock _playerScores;
    private List<Button> _availableButtons = new List<Button>();
    private int _currentButtonActive;

    public int ButtonCount { get; private set; }

    public HighScoreWindow() : base(0, 0, GameEngine.GameScreenHeight, GameEngine.GameScreenWidth, '█')
    {
        List<string> highScoreMsg = new List<string>
        {
            @" __    __   __    _______  __    __          _______.  ______   ______   .______       _______ 
",
            @"|  |  |  | |  |  /  _____||  |  |  |        /       | /      | /  __  \  |   _  \     |   ____|
",
            @"|  |__|  | |  | |  |  __  |  |__|  |       |   (----`|  ,----'|  |  |  | |  |_)  |    |  |__   
",
            @"|   __   | |  | |  | |_ | |   __   |        \   \    |  |     |  |  |  | |      /     |   __|  
",
            @"|  |  |  | |  | |  |__| | |  |  |  |    .----)   |   |  `----.|  `--'  | |  |\  \----.|  |____ 
",
            @"|__|  |__| |__|  \______| |__|  |__|    |_______/     \______| \______/  | _| `._____||_______|
"
        };


        _highScoreText = new TextBlock(2, 2, 100, highScoreMsg);
        _playerScores = new TextBlock((100 - 50) / 2, 11, 50, GetScoreList());

        _availableButtons.Add(new Button(8, 15, 7, 20, "Back to menu"));
        _availableButtons.Add(new Button(72, 15, 7, 20, "Exit game"));

        ButtonCount = _availableButtons.Count;

        _currentButtonActive = 0;
        _availableButtons[_currentButtonActive].SetActive();
    }

    public override void Render()
    {
        Console.Clear();
        
        base.Render();
        _highScoreText.Render();
        _playerScores.Render();

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

    private List<string> GetScoreList()
    {
        HighScoreManager hsm = new HighScoreManager();
        return hsm.GetHighScore();
    }
}
