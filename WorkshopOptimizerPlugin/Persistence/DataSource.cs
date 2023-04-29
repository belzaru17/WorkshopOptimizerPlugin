using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorkshopOptimizerPlugin.Persistence.Internal;

namespace WorkshopOptimizerPlugin.Persistence;

internal class DataSource
{
    public DataSource(Configuration configuration)
        : this(configuration, new PersistentData(configuration)) { }

    public uint Version => data.Version;

    public DateTime SeasonStart => data.CurrentSeason.SeasonStart;

    public DateTime?[] DataCollectionTime => data.CurrentSeason.DataCollectionTime;

    public DynamicDataAdaptor CurrentDynamicData { get; private set; }

    public ProducedItemsAdaptor CurrentProducedItems { get; private set; }

    public DynamicDataAdaptor PreviousDynamicData { get; private set; }

    public ProducedItemsAdaptor PreviousProducedItems { get; private set; }

    public bool[] CurrentRestCycles => data.CurrentSeason.RestCycles;

    public IReadOnlyList<bool> PreviousRestCycles => Array.AsReadOnly(data.PreviousSeason.RestCycles);


    public void NextWeek()
    {
        data.PreviousSeason = data.CurrentSeason;
        data.CurrentSeason = new();
        data.CurrentSeason.InitializeDefaults(configuration);
        Reset();
    }

    public bool Save(string filename)
    {
        try
        {
            var dirname = Path.GetDirectoryName(filename);
            if (dirname != null)
            {
                Directory.CreateDirectory(dirname);
            }
        }
        catch { }
        try
        {
            var bakname = Path.ChangeExtension(filename, "bak");
            if (bakname != null)
            {
                File.Copy(filename, bakname, true);
            }
        }
        catch { }
        try
        {
            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions()
            {
                WriteIndented = true,
                Converters = {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
            });
            File.WriteAllText(filename, json);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public static DataSource Load(Configuration configuration, string filename)
    {
        try
        {
            var json = File.ReadAllText(filename);
            var data = JsonSerializer.Deserialize<PersistentData>(json, new JsonSerializerOptions()
            {
                Converters = {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
            });

            return new DataSource(configuration, data ?? new PersistentData(configuration));
        }
        catch
        {
            return new DataSource(configuration);
        }
    }

    private DataSource(Configuration configuration, PersistentData data)
    {
        this.configuration = configuration;
        this.data = data;
        Reset();
    }

    [MemberNotNull(nameof(CurrentDynamicData), nameof(CurrentProducedItems), nameof(PreviousDynamicData), nameof(PreviousProducedItems))]
    private void Reset()
    {
        CurrentDynamicData = new DynamicDataAdaptor(data.CurrentSeason);
        CurrentProducedItems = new ProducedItemsAdaptor(data.CurrentSeason);
        PreviousDynamicData = new DynamicDataAdaptor(data.PreviousSeason);
        PreviousProducedItems = new ProducedItemsAdaptor(data.PreviousSeason);
    }

    private readonly Configuration configuration;
    private readonly PersistentData data;
}
