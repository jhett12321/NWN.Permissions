using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserRemovePermissionCommand : IChatCommand
  {
    private readonly ConfigService configService;

    public string Command => configService.GetFullChatCommand("user removepermission");
    public string[] Aliases => null;

    public Dictionary<string, object> UserData { get; } = new Dictionary<string, object>
    {
      [PermissionConstants.ChatUserDataKey] = PermissionConstants.UserRemovePermission,
    };

    public int? ArgCount => 1;
    public string Description => "Removes a permission from a user.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<permission_name>", "Remove the specified permission from the target user."),
    };

    public UserRemovePermissionCommand(ConfigService configService)
    {
      this.configService = configService;
    }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string permission = args[0];
      caller.EnterTargetMode(eventData => eventData.ProcessOnValidPCTargetBy(caller, RemoveUserPermissionToTarget(permission)));
    }

    private Action<NwPlayer, NwPlayer> RemoveUserPermissionToTarget(string permission)
    {
      return (NwPlayer target, NwPlayer caller) =>
      {
        configService.UpdateUserConfig(config =>
        {
          if (!config.UsersCd.TryGetValue(target.CDKey, out UserEntry entry))
          {
            entry = new UserEntry();
            config.UsersCd[target.CDKey] = entry;
          }

          if (!entry.Permissions.Contains(permission))
          {
            caller.SendErrorMessage($"No such permission on target: \"{permission}\"");
            return;
          }

          entry.Permissions ??= new List<string>();
          entry.Permissions.Remove(permission);
          caller.SendServerMessage($"Permission removed: \"{permission}\"", ColorConstants.Lime);
        });
      };
    }
  }
}
