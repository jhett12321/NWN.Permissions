using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
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
      ListAllUserPermissionsOfSelectedTarget(caller);
    }

    private void ListAllUserPermissionsOfSelectedTarget(NwPlayer caller)
    {
      caller.EnterTargetMode(OnCreatureSelection);

      void OnCreatureSelection(ModuleEvents.OnPlayerTarget eventData)
      {
        if (eventData.IsCancelled)
        {
          return;
        }

        if (!(eventData.TargetObject is NwCreature targetCreature &&
              targetCreature.IsLoginPlayerCharacter(out NwPlayer targetPlayer)))
        {
          eventData.Player.SendErrorMessage("Target must be a player character.");
          return;
        }

        PermissionSet userPermissions = ConfigService.GetPermissionsForPlayer(targetPlayer);
        caller.SendServerMessage($"{targetPlayer.CDKey} ({targetCreature.Name}) has {(userPermissions.Permissions.Count == 0 ? "no permissions." : "the following permissions:")}", ColorConstants.White);
        caller.SendServerMessage(String.Join("\n", userPermissions.Permissions), ColorConstants.White);
      }
    }
  }
}
