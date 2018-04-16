using System;

class Window : GuiObject
{
    private Frame _border;
    public bool IsAlreadyOpen { get; set; }

    public Window(int x, int y, int height, int width, char frameSymbol) : base(x, y, height, width)
    {
        _border = new Frame(x, y, height, width, frameSymbol);
        IsAlreadyOpen = false;
    }

    public virtual void Render()
    {
        _border.Render();
        Console.SetCursorPosition(0, 0);
    }
}
