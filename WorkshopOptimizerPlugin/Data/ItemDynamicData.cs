namespace WorkshopOptimizerPlugin.Data;

internal class ItemDynamicData
{
    public Popularity Popularity { get; set; }

    public Popularity NextPopularity { get; set; }

    public Supply[] Supply { get; set; }
    public Demand[] Demand { get; set; }

    public ItemDynamicData() : this(Popularity.Unknown, Popularity.Unknown, new Supply[7], new Demand[7]) { }

    public ItemDynamicData(Popularity popularity, Popularity nextPopularity, Supply[] supply, Demand[] demand)
    {
        this.Popularity = popularity;
        this.NextPopularity = nextPopularity;
        this.Supply = supply;
        this.Demand = demand;
    }
}
