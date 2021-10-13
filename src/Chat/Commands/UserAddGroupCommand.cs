using Anvil.API;

namespace Jorteck.Permissions
{
  internal class UserAddGroupCommand : ICommand
  {
    private readonly PermissionsConfigService permissionsConfigService;

    public string Command { get; }
    public string Permission { get; }
    public int? ArgCount { get; }
    public string Description { get; }
    public string[] Usages { get; }

    public UserAddGroupCommand(PermissionsConfigService permissionsConfigService)
    {
      this.permissionsConfigService = permissionsConfigService;
      Command = "group adduser";
      Permission = PermissionConstants.UserAddGroup;
      ArgCount = null;
      Description = "Adds a group membership to a user.";
      Usages = new[]
      {
        $"{Command} <group_name>    You will be prompted to select the player with your cursor.",
      };
    }

    public void ProcessCommand(NwPlayer caller, string[] args)
    {

    }
  }
}
