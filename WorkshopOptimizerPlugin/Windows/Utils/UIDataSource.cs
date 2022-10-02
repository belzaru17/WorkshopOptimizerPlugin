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
    public ItemCache ItemCache => itemCache;
    public bool Dirty { get; private set; } = false;

    public UIDataSource()
    {
        Reset();
    }

    public static UIDataSource Load()
    {
        var path = Environment.ExpandEnvironmentVariables(JsonFileBaseName);
        return new UIDataSource(DataSource.Load(path));
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

    [MemberNotNull(nameof(dataSource), nameof(whenOverrides), nameof(itemCache))]
    public void Reset()
    {
        reset(new DataSource());
    }

    public void Reload()
    {
        var path = Environment.ExpandEnvironmentVariables(JsonFileBaseName);
        reset(DataSource.Load(path));
    }

    public bool Save()
    {
        Dirty = false;
        var path = Environment.ExpandEnvironmentVariables(JsonFileBaseName);
        return DataSource.Save(path);
    }

    private UIDataSource(DataSource dataSource)
    {
        reset(dataSource);
    }

    [MemberNotNull(nameof(dataSource), nameof(whenOverrides), nameof(itemCache))]
    private void reset(DataSource dataSource)
    {
        this.dataSource = dataSource;
        whenOverrides = new WhenOverrides();
        itemCache = new ItemCache(dataSource, whenOverrides);

        DataChanged();
        Dirty = false;
    }

    private const string JsonFileBaseName = @"%AppData%\\WorkshopOptimizerPlugin";

    private DataSource dataSource;
    private WhenOverrides whenOverrides;
    private ItemCache itemCache;
    private List<IUIDataSourceListener> listeners = new();
}
