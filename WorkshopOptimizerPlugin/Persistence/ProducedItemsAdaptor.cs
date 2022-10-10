using WorkshopOptimizerPlugin.Data;
using WorkshopOptimizerPlugin.Persistence.Internal;

namespace WorkshopOptimizerPlugin.Persistence;

internal class ProducedItemsAdaptor
{
    public ProducedItemsAdaptor(PersistentSeasonData data)
    {
        this.data = data;

        itemsByCycle = new int[Constants.MaxCycles, Constants.MaxItems];
        for (int c = 0; c < Constants.MaxCycles; c++)
        {
            for (int w = 0; w < Constants.MaxWorkshops; w++)
            {
                for (int s = 0; s < Constants.MaxSteps; s++)
                {
                    var id = this.data.ProducedItems[c][w][s];
                    if (id >= 0)
                    {
                        itemsByCycle[c, id] += (s == 0) ? 1 : 2;
                    }
                }
            }
        }

        GrooveAtEndOfCycle = new Groove[Constants.MaxCycles];
        UpdateGrooveByCycle();
    }

    public int this[int cycle, int workshop, int step] {
        get { return data.ProducedItems[cycle][workshop][step]; }
        set {
            var prevId = data.ProducedItems[cycle][workshop][step];
            if (prevId == value) { return; }
            if (prevId >= 0)
            {
                itemsByCycle[cycle, prevId] -= (step == 0) ? 1 : 2;
            }
            data.ProducedItems[cycle][workshop][step] = value;
            if (value >= 0)
            {
                itemsByCycle[cycle, value] += (step == 0) ? 1 : 2;
            }
            UpdateGrooveByCycle();
        }
    }

    public Groove[] GrooveAtEndOfCycle { get; private set; }

    public int ItemsProducedUntil(int cycle, uint itemId)
    {
        var sum = 0;
        for (int c = 0; c < cycle; c++)
        {
            sum += itemsByCycle[c, itemId];
        }
        return sum;
    }

    private void UpdateGrooveByCycle()
    {
        var groove = new Groove();
        for (int c = 0; c < Constants.MaxCycles; c++)
        {
            for (int w = 0; w < Constants.MaxWorkshops; w++)
            {
                for (int s = 0; s < Constants.MaxSteps; s++)
                {
                    var id = this.data.ProducedItems[c][w][s];
                    if (id >= 0)
                    {
                        groove = groove.Inc(s);
                    }
                }
            }
            GrooveAtEndOfCycle[c] = groove;
        }
    }

    private PersistentSeasonData data;
    private int[,] itemsByCycle;
}
