namespace WorkshopOptimizerPlugin.Data;

internal class Groove(int value)
{
    public int Value { get; init; } = value > Constants.MaxGroove ? Constants.MaxGroove : value;

    public Groove() : this(0) { }

    public override string ToString() => Value.ToString();

    public Groove Inc(int step, int count = 1)
    {
        if (step == 0) { return this; }
        return new Groove(Value + count);
    }

    public Groove Dec(int step, int count = 1)
    {
        if (step == 0) { return this; }
        return new Groove(Value - count);
    }

    public double Multiplier()
    {
        return 1.0 + ((double)Value / 100.0);
    }
}
