using System;
using System.Collections.Generic;

class TextBlock : GuiObject
{
    private readonly List<TextLine> _textBlocks = new List<TextLine>();

    // no height - pass 0
    public TextBlock(int x, int y, int width, List<string> text) : base(x, y, 0, width)
    {
        for (int i = 0; i < text.Count; i++)
        {
            // add TextLine to a new (y + 1) row
            _textBlocks.Add(new TextLine(x, y + i, width, text[i]));
        }
    }

    public void Render()
    {
        for (int i = 0; i < _textBlocks.Count; i++)
        {
            _textBlocks[i].Render();
        }
    }

    public void Render(ConsoleColor color)
    {
        for (int i = 0; i < _textBlocks.Count; i++)
        {
            _textBlocks[i].Render(color);
        }
    }

}
