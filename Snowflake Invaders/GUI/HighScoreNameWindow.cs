using System;

class HighScoreNameWindow : Window
{
    private Button _okButton;
    private TextLine _enterPlayerName;

    public HighScoreNameWindow() : base(20, 3, 20, 60, '█')
    {

        _enterPlayerName = new TextLine(20 + 2, 3 + 3, 60 - 4, "Enter your name:");

        _okButton = new Button(40, 12, 7, 20, "Back");
        _okButton.SetActive();

    }

    public override void Render()
    {
        base.Render();

        _enterPlayerName.Render();
        _okButton.Render();

        Console.SetCursorPosition(0, 0);
    }

}
