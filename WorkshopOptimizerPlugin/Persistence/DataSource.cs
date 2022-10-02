using System;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using WorkshopOptimizerPlugin.Persistence.Internal;

namespace WorkshopOptimizerPlugin.Persistence;

internal class DataSource
{
    public DataSource() : this(new PersistentData()) { }

    public uint Version => data.Version;

    public DateTime SeasonStart => data.SeasonStart;

    public DateTime?[] DataCollectionTime => data.DataCollectionTime;

    public DynamicDataAdaptor DynamicData { get; init; }

    public ProducedItemsAdaptor ProducedItems { get; init; }

    public bool Save(string basename)
    {
        try
        {
            File.Copy(basename + JsonExtension, basename + BackupExtension, true);
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
            File.WriteAllText(basename + JsonExtension, json);
        }
        catch
        {
            return false;
        }
        return true;
    }

    public static DataSource Load(string basename)
    {
        try
        {
            var json = File.ReadAllText(basename + JsonExtension);
            var data = JsonSerializer.Deserialize<PersistentData>(json, new JsonSerializerOptions()
            {
                Converters = {
                    new JsonStringEnumConverter(JsonNamingPolicy.CamelCase)
                },
            });

            return new DataSource(data ?? new PersistentData());
        }
        catch
        {
            return new DataSource();
        }
    }

    private DataSource(PersistentData data)
    {
        this.data = data;
        DynamicData = new DynamicDataAdaptor(data);
        ProducedItems = new ProducedItemsAdaptor(data);
    }

    private PersistentData data;

    static private readonly string JsonExtension = ".json";
    static private readonly string BackupExtension = ".bak";
}
