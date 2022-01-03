using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserAddPermissionCommand : IChatCommand
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Command => ConfigService.GetFullChatCommand("user addpermission");
    public string[] Aliases => null;

    public Dictionary<string, object> UserData { get; } = new Dictionary<string, object>
    {
      [PermissionConstants.ChatUserDataKey] = PermissionConstants.UserAddPermission,
    };

    public int? ArgCount => 1;
    public string Description => "Grants a permission to a user.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<permission_name>", "Grant the specified permission to the target user."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string permission = args[0];
      caller.EnterPlayerTargetMode(selection => AddUserPermissionToTarget(selection, permission));
    }

    private void AddUserPermissionToTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection, string permission)
    {
      NwPlayer caller = selection.Caller;
      NwPlayer target = selection.Target;

      ConfigService.UpdateUserConfig(config =>
      {
        if (!config.UsersCd.TryGetValue(target.CDKey, out UserEntry entry))
        {
          entry = new UserEntry();
          config.UsersCd[target.CDKey] = entry;
        }

        if (entry.Permissions.Contains(permission))
        {
          caller.SendErrorMessage($"Target already has permission \"{permission}\"");
          return;
        }

        entry.Permissions ??= new List<string>();
        entry.Permissions.Add(permission);
        caller.SendServerMessage($"Permission granted: \"{permission}\"", ColorConstants.Lime);
      });
    }
  }
}
