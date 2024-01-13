namespace WorkshopOptimizerPlugin.Data;

internal class ItemDynamicData(Popularity popularity, Popularity nextPopularity, Supply[] supply, Demand[] demand)
{
    public Popularity Popularity { get; set; } = popularity;

    public Popularity NextPopularity { get; set; } = nextPopularity;

    public Supply[] Supply { get; set; } = supply;
    public Demand[] Demand { get; set; } = demand;

    public ItemDynamicData() : this(Popularity.Unknown, Popularity.Unknown, new Supply[7], new Demand[7]) { }
}
