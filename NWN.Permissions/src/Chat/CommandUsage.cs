namespace Jorteck.Permissions
{
  internal sealed class CommandUsage
  {
    public string SubCommand { get; }
    public string Description { get; }

    public CommandUsage(string subCommand, string description)
    {
      SubCommand = subCommand;
      Description = description;
    }

    public CommandUsage(string description)
    {
      Description = description;
    }
  }
}
