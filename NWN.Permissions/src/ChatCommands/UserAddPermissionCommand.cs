using System;
using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserAddPermissionCommand : IChatCommand
  {
    private readonly ConfigService configService;

    public string Command => configService.GetFullChatCommand("user addpermission");
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

    public UserAddPermissionCommand(ConfigService configService)
    {
      this.configService = configService;
    }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string permission = args[0];
      caller.EnterPlayerTargetMode(selection => AddUserPermissionToTarget(selection, permission));
    }

    private void AddUserPermissionToTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection, string permission)
    {
      var caller = selection.Caller;
      var target = selection.Target;

      configService.UpdateUserConfig(config =>
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
