class HeroChar : Unit
{
    public HeroChar(UnitCoord coord, char symbol) : base(coord, symbol)
    {
    }

    public int MoveRight()
    {
        return ++_coord.X;
    }

    public int MoveLeft()
    {
        return --_coord.X;
    }

    public int MoveUp()
    {
        return --_coord.Y;
    }

    public int MoveDown()
    {
        return ++_coord.Y;
    }
}
