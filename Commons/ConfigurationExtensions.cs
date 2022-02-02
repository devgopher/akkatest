using Akka.Configuration;
using Akka.Configuration.Hocon;
using Microsoft.Extensions.Configuration;

namespace Commons;

public static class ConfigurationExtensions
{
    public static Config ToAkkaConfiguration(this IConfiguration config)
    {
        var root = new HoconValue();
        ParseConfig(root, config);
        var hconRoot = new HoconRoot(root);
        return new Config(hconRoot);
    }

    private static void ParseConfig(HoconValue owner, IConfiguration config)
    {
        if (config is IConfigurationSection section && !config.GetChildren().Any())
        {
            var lit = new HoconLiteral { Value = section.Value };
            owner.AppendValue(lit);
        }
        else
        {
            foreach (var child in config.GetChildren())
            {
                if (child.Path.EndsWith(":0"))
                {
                    var array = ParseArray(config);
                    owner.AppendValue(array);
                    return;
                }
                else
                {
                    if (owner.GetObject() == null)
                        owner.NewValue(new HoconObject());

                    var key = owner.GetObject().GetOrCreateKey(child.Key);

                    ParseConfig(key, child);
                }
            }
        }
    }

    private static HoconArray ParseArray(IConfiguration config)
    {
        var arr = new HoconArray();

        foreach (var arrayChild in config.GetChildren())
        {
            var value = new HoconValue();
            ParseConfig(value, arrayChild);
            arr.Add(value);
        }

        return arr;
    }
}