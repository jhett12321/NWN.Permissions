using System.Collections.Generic;

namespace Jorteck.Permissions
{
  internal sealed class UserConfig
  {
    public const string ConfigName = "users.yml";

    public Dictionary<string, UserEntry> Users = new Dictionary<string, UserEntry>();
  }

  internal sealed class UserEntry
  {
    public IdType IdType { get; set; } = IdType.cd_key;
    public string Username { get; set; }
    public string Character { get; set; }
    public List<string> Groups { get; set; } = new List<string>();
    public List<string> Permissions { get; set; } = new List<string>();
    public Dictionary<string, string> Info { get; set; } = new Dictionary<string, string>();
  }

  internal enum IdType
  {
    cd_key,
    character_id,
    username,
  }
}
