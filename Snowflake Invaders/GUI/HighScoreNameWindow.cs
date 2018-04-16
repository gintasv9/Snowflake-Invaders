using System;

class HighScoreNameWindow : Window
{
    private Button _okButton;
    private TextLine _enterPlayerName;

    public string PlayerName { get; set; }

    public HighScoreNameWindow() : base(20, 3, 20, 60, '█')
    {

        _enterPlayerName = new TextLine(20 + 1, 3 + 1, 60 - 2, "Enter your name:");

        _okButton = new Button(40, 10, 7, 20, "Back");
        _okButton.SetActive();

    }

    public override void Render()
    {
        base.Render();

        _enterPlayerName.Render();
        _okButton.Render();

        Console.SetCursorPosition(0, 0);
    }

    public void AskForUserInput()
    {
        Console.SetCursorPosition(45, 7);
        Console.CursorVisible = true;
        PlayerName = Console.ReadLine();
        Console.CursorVisible = false;
    }
}
