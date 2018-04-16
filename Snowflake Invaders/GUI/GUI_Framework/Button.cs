class Button : GuiObject
{
    private TextLine _buttonName;

    private Frame _activeFrame;
    private Frame _notActiveFrame;

    private bool _isActive = false;

    public Button(int x, int y, int height, int width, string buttonText) : base(x, y, height, width)
    {
        // paduodami parametrai, kad mygtuko tekstas butu centre
        _buttonName = new TextLine(x + 1, y + 1 + ((height - 2) / 2), width - 2, buttonText);

        _activeFrame = new Frame(x, y, height, width, '▓');
        _notActiveFrame = new Frame(x, y, height, width, '░');
    }

    public void Render()
    {
        if (_isActive)
        {
            _activeFrame.Render();
        }
        else
        {
            _notActiveFrame.Render();
        }

        _buttonName.Render();
    }

    public void SetActive()
    {
        _isActive = true;
    }

    public void SetInactive()
    {
        _isActive = false;
    }

    public bool IsButtonActive()
    {
        return _isActive;
    }
}
