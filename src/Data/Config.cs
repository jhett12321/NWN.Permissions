namespace Jorteck.Permissions
{
  internal sealed class Config
  {
    public const string ConfigName = "config.yml";

    public bool ChatCommandEnable { get; set; } = true;
    public string ChatCommand { get; set; } = "/perms";
    public int Version { get; set; } = 1;
  }
}
