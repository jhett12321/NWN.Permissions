using System.IO;
using Anvil;
using Anvil.Services;
using NLog;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(PermissionsService))]
  public sealed class PermissionsService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private static readonly string PluginPath = Path.Combine(Path.GetDirectoryName(typeof(AnvilCore).Assembly.Location), "Plugins/NWN.Permissions");

    private readonly IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .Build();

    private readonly ISerializer serializer = new SerializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .Build();

    private Config config;
    private GroupConfig groupConfig;
    private UserConfig userConfig;

    public PermissionsService()
    {
      LoadAllConfigsFromDisk();
    }

    private void LoadAllConfigsFromDisk()
    {
      config = LoadConfig<Config>(Config.ConfigName);
      groupConfig = LoadConfig<GroupConfig>(GroupConfig.ConfigName);
      userConfig = LoadConfig<UserConfig>(UserConfig.ConfigName);
    }

    private T LoadConfig<T>(string fileName) where T : new()
    {
      string configPath = GetConfigPath(fileName);
      T retVal;

      if (!File.Exists(configPath))
      {
        retVal = new T();
        SaveConfig<T>(fileName, retVal);
      }
      else
      {
        retVal = deserializer.Deserialize<T>(File.ReadAllText(configPath));
      }

      return retVal;
    }

    private void SaveConfig<T>(string fileName, T instance)
    {
      string yaml = serializer.Serialize(instance);
      File.WriteAllText(GetConfigPath(fileName), yaml);
    }

    private string GetConfigPath(string fileName)
    {
      return Path.Combine(PluginPath, fileName);
    }
  }
}
