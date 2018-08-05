using System;

class TextLine : GuiObject
{
    private readonly string _data;

    public TextLine(int x, int y, int width, string textLine) : base(x, y, 0, width)
    {
        _data = textLine;
    }

    public void Render()
    {
        Console.SetCursorPosition(_x, _y);

        // center if text length < window width
        if (_width > _data.Length)
        {
            int offset = (_width - _data.Length) / 2;
            for (int i = 0; i < offset; i++)
            {
                Console.Write(' ');
            }
        }

        Console.WriteLine(_data);
    }

    public void Render(ConsoleColor color)
    {
        Console.SetCursorPosition(_x, _y);

        // center if text length < window width
        if (_width > _data.Length)
        {
            int offset = (_width - _data.Length) / 2;
            for (int i = 0; i < offset; i++)
            {
                Console.Write(' ');
            }
        }

        Console.ForegroundColor = color;
        Console.WriteLine(_data);
        Console.ResetColor();
    }
}
