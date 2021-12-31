using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserListPermsCommand : IChatCommand
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Command => ConfigService.GetFullChatCommand("user listpermissions");
    public string[] Aliases => null;
    public Dictionary<string, object> UserData { get; } = new Dictionary<string, object>
    {
      [PermissionConstants.ChatUserDataKey] = PermissionConstants.List,
    };
    public int? ArgCount => 0;
    public string Description => "Lists all user permissions.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("List all permissions for the target user."),
    };

    public UserListPermsCommand() { }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      caller.EnterTargetMode(eventData => eventData.ProcessOnValidPCTargetBy(caller, ListPermissionsOfTargetToCaller));
    }

    private void ListPermissionsOfTargetToCaller(NwPlayer target, NwPlayer caller)
    {
      PermissionSet userPermissions = ConfigService.GetPermissionsForPlayer(target);
      caller.SendServerMessage($"Target has {(userPermissions.Permissions.Count == 0 ? "no permissions." : "the following permissions:")}", ColorConstants.Orange);
      caller.SendServerMessage(String.Join("\n", userPermissions.Permissions), ColorConstants.Lime);
    }
  }
}
