using System;
using System.Collections.Generic;

class TextBlock : GuiObject
{
    private readonly List<TextLine> _textBlocks = new List<TextLine>();

    // kadangi height nera - siunciamas 0
    public TextBlock(int x, int y, int width, List<string> text) : base(x, y, 0, width)
    {
        for (int i = 0; i < text.Count; i++)
        {
            // i textBlocks lista pridedame NAUJUS TextLine; y + 1 -> sekanti konsoles eilute
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
