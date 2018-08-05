using System;
using System.Collections.Generic;

sealed class CreditWindow : Window
{
    private Button _backButton;
    private TextBlock _creditTextBlock;

    public CreditWindow() : base(20, 3, 20, 60, '█')
    {
        List<string> creditInfo = new List<string>
        {
            "Game design:",
            "Gintautas Vasauskas",
            "",
            "Music:",
            "vikce - 'casse-tête'",
            "M.O.O.N. - 'Hydrogen'",
            "M.O.O.N. - 'Dust'",
            "",
            "Supervisor:",
            "Raimundas Banevicius",
            "",
            "2018 © Vilnius Coding School"
        };

        _creditTextBlock = new TextBlock(20 + 2, 3 + 2, 60 - 4, creditInfo);

        _backButton = new Button(40, creditInfo.Count + 6, 3, 20, "Back");
        _backButton.SetActive();

    }

    public override void Render()
    {
        base.Render();

        _creditTextBlock.Render();
        _backButton.Render();

        Console.SetCursorPosition(0, 0);
    }
}
