abstract class GuiObject
    {
        protected int _x;
        protected int _y;
        protected int _height;
        protected int _width;

        public GuiObject(int x, int y, int height, int width)
        {
            this._x = x;
            this._y = y;
            this._height = height;
            this._width = width;
        }
    }
