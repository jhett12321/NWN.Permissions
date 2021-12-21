using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBindingOptions(BindingPriority = BindingPriority.Low, PluginDependencies = new[] { "NWN.ChatTools" })]
  internal sealed class PermissionsCommandListProvider : CommandListProvider
  {
    private readonly PermissionsService permissionsService;
    private readonly HelpCommand helpCommand;

    public PermissionsCommandListProvider(PermissionsService permissionsService, HelpCommand helpCommand)
    {
      this.permissionsService = permissionsService;
      this.helpCommand = helpCommand;
    }

    public override bool CanUseCommand(NwPlayer player, IChatCommand command)
    {
      if (command == helpCommand)
      {
        return permissionsService.HasPermission(player, PermissionConstants.Help);
      }

      if (command.UserData?.TryGetValue(PermissionConstants.ChatUserDataKey, out object permissionKey) == true)
      {
        return permissionsService.HasPermission(player, permissionKey.ToString());
      }

      return base.CanUseCommand(player, command);
    }
  }
}
