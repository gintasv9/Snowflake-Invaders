using System;
using System.Collections.Generic;

class MenuWindow : Window
{
    private TextBlock _menuControls;
    private TextBlock _gameControls;
    private List<Button> _availableButtons = new List<Button>();
    private int _currentButtonActive;

    public int ButtonCount { get; private set; }

    public MenuWindow() : base(0, 0, GameEngine.GameScreenHeight, GameEngine.GameScreenWidth, '█')
    {
        _menuControls = new TextBlock(20, 4, 60, new List<string> { "Menu controls:", "Choose menu item: ← →", "Select: ENTER" });
        _gameControls = new TextBlock(20, 8, 60, new List<string> { "Game controls:", "Move: ↑ ↓ ← →", "Shoot: SPACEBAR"  });

        _availableButtons.Add(new Button(8, 4, 7, 20, "Credits"));
        _availableButtons.Add(new Button(8, 14, 7, 20, "High Score"));
        _availableButtons.Add(new Button(40, 14, 7, 20, "START!"));
        _availableButtons.Add(new Button(72, 14, 7, 20, "Sound ON / OFF"));
        _availableButtons.Add(new Button(72, 4, 7, 20, "Exit game"));

        ButtonCount = _availableButtons.Count;

        _currentButtonActive = 2;
        _availableButtons[_currentButtonActive].SetActive();
    }

    public override void Render()
    {
        base.Render();
        _menuControls.Render(ConsoleColor.Green);
        _gameControls.Render(ConsoleColor.Cyan);
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
