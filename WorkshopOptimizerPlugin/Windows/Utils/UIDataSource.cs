using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using WorkshopOptimizerPlugin.Optimizer;
using WorkshopOptimizerPlugin.Persistence;

namespace WorkshopOptimizerPlugin.Windows.Utils;

internal interface IUIDataSourceListener
{
    public void OnOptimizationParameterChange();
    public void OnDataChange(int cycle);
}

internal class UIDataSource
{
    public DataSource DataSource => dataSource;
    public WhenOverrides WhenOverrides => whenOverrides;
    public ItemCache CurrentItemCache => currentItemCache;
    public ItemCache PreviousItemCache => previousItemCache;
    public bool Dirty { get; private set; } = false;

    public static UIDataSource Load(Configuration configuration)
    {
        var path = Environment.ExpandEnvironmentVariables(JsonFileName);
        return new UIDataSource(DataSource.Load(configuration, path));
    }

    public void OptimizationParameterChanged()
    {
        foreach (var listener in listeners)
        {
            listener.OnOptimizationParameterChange();
        }
    }

    public void DataChanged(int cycle = 0)
    {
        Dirty = true;
        foreach (var listener in listeners)
        {
            listener.OnDataChange(cycle);
        }
    }

    public void AddListener(IUIDataSourceListener listener)
    {
        listeners.Add(listener);
    }

    [MemberNotNull(nameof(dataSource), nameof(whenOverrides), nameof(currentItemCache), nameof(previousItemCache))]
    public void Reset(Configuration configuration)
    {
        Reset(new DataSource(configuration));
    }

    public void NextWeek()
    {
        dataSource.NextWeek();
        Reset(dataSource);
    }

    public bool Save()
    {
        Dirty = false;
        var path = Environment.ExpandEnvironmentVariables(JsonFileName);
        return DataSource.Save(path);
    }

    private UIDataSource(DataSource dataSource)
    {
        Reset(dataSource);
    }

    [MemberNotNull(nameof(dataSource), nameof(whenOverrides), nameof(currentItemCache), nameof(previousItemCache))]
    private void Reset(DataSource dataSource)
    {
        this.dataSource = dataSource;
        whenOverrides = new WhenOverrides();
        currentItemCache = new ItemCache(dataSource.CurrentDynamicData, dataSource.CurrentProducedItems, whenOverrides);
        previousItemCache = new ItemCache(dataSource.PreviousDynamicData, dataSource.PreviousProducedItems, whenOverrides);

        DataChanged();
        Dirty = false;
    }

    private const string JsonFileName = @"%AppData%\\WorkshopOptimizerPlugin\\WorkshopOptimizerPlugin.json";

    private DataSource dataSource;
    private WhenOverrides whenOverrides;
    private ItemCache currentItemCache;
    private ItemCache previousItemCache;
    private readonly List<IUIDataSourceListener> listeners = new();
}
