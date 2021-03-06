﻿using System;
using System.Text;

class Frame : GuiObject
{
    private char _renderChar;

    public Frame(int x, int y, int height, int width, char frameSymbol) : base(x, y, height, width)
    {
        _renderChar = frameSymbol;
    }

    public void Render()
    {
        for (int i = 0; i < _height; i++)
        {
            Console.SetCursorPosition(_x, _y + i);
            if (i == 0)
            {
                for (int j = 0; j < _width; j++)
                {
                    Console.Write(_renderChar);
                }
            }
            else if (i == _height - 1)
            {
                for (int j = 0; j < _width - 1; j++)
                {
                    Console.Write(_renderChar);
                }

                // bugfix for console cursor adding a new empty line on frame's corner coord:
                Console.OutputEncoding = Encoding.GetEncoding(850);
                Console.SetCursorPosition(_x, _y);
                Console.MoveBufferArea(_x, _y, 1, 1, _x + _width - 1, _y + _height - 1,
                    _renderChar, Console.ForegroundColor, Console.BackgroundColor);
            }
            else
            {
                Console.Write(_renderChar);
                Console.Write(_renderChar);

                for (int j = 1; j < _width - 3; j++)
                {
                    Console.Write(' ');
                }   

                Console.Write(_renderChar);
                Console.Write(_renderChar);
            }
        }
        Console.SetCursorPosition(0, 0);
        Console.OutputEncoding = Encoding.UTF8;
    }
}
