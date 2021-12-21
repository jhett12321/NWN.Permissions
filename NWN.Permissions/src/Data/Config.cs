using System;
using System.ComponentModel;

namespace Jorteck.Permissions
{
  [Serializable]
  internal sealed class Config
  {
    public const string ConfigName = "config.yml";

    [Description("Adds support for managing permissions with chat commands. Requires the NWN.ChatTools plugin.")]
    public bool ChatCommandEnable { get; set; } = true;

    [Description("The base chat command name.")]
    public string ChatCommand { get; set; } = "perms";

    public int Version { get; set; } = 1;
  }
}
