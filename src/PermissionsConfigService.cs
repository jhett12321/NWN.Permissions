using System.Collections.Generic;
using System.IO;
using System.Linq;
using Anvil;
using Anvil.API;
using Anvil.Services;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(PermissionsConfigService))]
  public sealed class PermissionsConfigService
  {
    private static readonly string PluginPath = Path.Combine(Path.GetDirectoryName(typeof(AnvilCore).Assembly.Location), "Plugins/NWN.Permissions");

    private readonly IDeserializer deserializer = new DeserializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .Build();

    private readonly ISerializer serializer = new SerializerBuilder()
      .WithNamingConvention(UnderscoredNamingConvention.Instance)
      .Build();

    private List<GroupEntry> defaultGroups;
    private List<GroupEntry> defaultDMGroups;
    private Dictionary<NwPlayer, PermissionSet> cachedPermissions = new Dictionary<NwPlayer, PermissionSet>();

    private Config config;
    private GroupConfig groupConfig;
    private UserConfig userConfig;

    public PermissionsConfigService()
    {
      LoadAllConfigsFromDisk();
    }

    public PermissionSet GetPermissionsForPlayer(NwPlayer player)
    {
      if (!cachedPermissions.TryGetValue(player, out PermissionSet permissionSet))
      {
        permissionSet = CreatePermissionSet(player);
        cachedPermissions[player] = permissionSet;
      }

      return permissionSet;
    }

    private PermissionSet CreatePermissionSet(NwPlayer player)
    {
      PermissionSet permissionSet = new PermissionSet();
      UserEntry userEntry = ResolveUserEntry(player);
      List<GroupEntry> groupEntries = ResolveGroupEntries(player, userEntry);

      if (userEntry != null)
      {
        ParsePermissionEntries(permissionSet, userEntry.Permissions);
      }

      foreach (GroupEntry groupEntry in groupEntries)
      {
        ParsePermissionEntries(permissionSet, groupEntry.Permissions);
      }

      cachedPermissions[player] = permissionSet;
      return permissionSet;
    }

    private UserEntry? ResolveUserEntry(NwPlayer player)
    {
      UserEntry userEntry;

      if (userConfig.UsersCd.TryGetValue(player.CDKey, out userEntry))
      {
        return userEntry;
      }

      if (userConfig.UsersCharacter.TryGetValue(player.LoginCreature.UUID.ToUUIDString(), out userEntry))
      {
        return userEntry;
      }

      if (userConfig.UsersUsername.TryGetValue(player.PlayerName, out userEntry))
      {
        return userEntry;
      }

      return null;
    }

    private List<GroupEntry> ResolveGroupEntries(NwPlayer player, UserEntry? userEntry)
    {
      if (userEntry == null)
      {
        return !player.IsDM ? defaultGroups : defaultDMGroups;
      }

      List<GroupEntry> groupEntries = !player.IsDM ? new List<GroupEntry>(defaultGroups) : new List<GroupEntry>(defaultDMGroups);
      foreach (string groupName in userEntry.Groups)
      {
        if (groupConfig.Groups.TryGetValue(groupName, out GroupEntry groupEntry))
        {
          AddInheritedGroups(groupEntries, groupEntry);
          groupEntries.Add(groupEntry);
        }
      }

      return groupEntries;
    }

    private void AddInheritedGroups(List<GroupEntry> destination, GroupEntry groupEntry)
    {
      foreach (string groupName in groupEntry.Inheritance)
      {
        if (groupConfig.Groups.TryGetValue(groupName, out GroupEntry inheritedGroup))
        {
          AddInheritedGroups(destination, inheritedGroup);
          destination.Add(inheritedGroup);
        }
      }
    }

    private void ParsePermissionEntries(PermissionSet destination, List<string> permissionEntries)
    {
      foreach (string entry in permissionEntries)
      {
        string permission;
        HashSet<string> targetSet;

        // Wildcard
        if (entry.EndsWith(".*"))
        {
          permission = entry[..^2];
          targetSet = destination.WildcardPermissions;
        }
        else if (entry.EndsWith('*'))
        {
          permission = entry[..^1];
          targetSet = destination.WildcardPermissions;
        }
        else
        {
          permission = entry;
          targetSet = destination.Permissions;
        }

        // Negation
        if (permission.StartsWith('-'))
        {
          targetSet.Remove(permission[1..]);
        }
        else
        {
          targetSet.Add(permission);
        }
      }
    }

    private void Refresh()
    {
      cachedPermissions.Clear();
      defaultDMGroups.Clear();

      foreach (KeyValuePair<string,GroupEntry> groupEntry in groupConfig.Groups)
      {
        if (groupEntry.Value.Default)
        {
          defaultGroups.Add(groupEntry.Value);
        }

        if (groupEntry.Value.DefaultDm)
        {
          defaultDMGroups.Add(groupEntry.Value);
        }
      }
    }

    private void LoadAllConfigsFromDisk()
    {
      config = LoadConfig<Config>(Config.ConfigName);
      groupConfig = LoadConfig<GroupConfig>(GroupConfig.ConfigName);
      userConfig = LoadConfig<UserConfig>(UserConfig.ConfigName);
      Refresh();
    }

    private T LoadConfig<T>(string fileName) where T : new()
    {
      string configPath = GetConfigPath(fileName);
      T retVal;

      if (!File.Exists(configPath))
      {
        retVal = new T();
        SaveConfig(fileName, retVal);
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
