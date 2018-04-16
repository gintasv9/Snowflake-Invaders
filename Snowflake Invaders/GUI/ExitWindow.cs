using System;
using System.Collections.Generic;

sealed class ExitWindow : Window
{
    private readonly TextLine _exitQuestion;

    private List<Button> _availableButtons = new List<Button>();
    private int _currentButtonActive;

    public int ButtonCount { get; private set; }

    public ExitWindow() : base (20, 10, 10, 60, '�')
    {
        _exitQuestion = new TextLine(_x + 1, 12, _width, "Do you really want to exit this application?");

        _availableButtons.Add(new Button(35, 15, 3, 11, "YES"));
        _availableButtons.Add(new Button(55, 15, 3, 11, "NO!"));

        ButtonCount = _availableButtons.Count;

        // No! - default pasirinkimas
        _currentButtonActive = 1;
        _availableButtons[_currentButtonActive].SetActive();
    }

    public override void Render()
    {
        base.Render();

        _exitQuestion.Render();

        RenderButtons();
    }

    public void RenderButtons()
    {
        for (int i = 0; i < ButtonCount; i++)
        {
            _availableButtons[i].Render();
        }

        Console.SetCursorPosition(0, 0);
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
