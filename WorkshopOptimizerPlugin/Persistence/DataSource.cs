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

    public static DataSource Load(string filename)
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
}
