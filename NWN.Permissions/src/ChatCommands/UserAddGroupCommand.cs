using System.Collections.Generic;
using Anvil.API;
using Anvil.Services;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(IChatCommand))]
  internal class UserAddGroupCommand : IChatCommand
  {
    [Inject]
    private ConfigService ConfigService { get; init; }

    public string Command => ConfigService.GetFullChatCommand("user addgroup");
    public string[] Aliases => null;

    public Dictionary<string, object> UserData { get; } = new Dictionary<string, object>
    {
      [PermissionConstants.ChatUserDataKey] = PermissionConstants.UserAddGroup,
    };

    public int? ArgCount => 1;
    public string Description => "Adds a group membership to a user.";

    public CommandUsage[] Usages { get; } =
    {
      new CommandUsage("<group_name>", "Add a player to the specified group."),
    };

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string group = args[0];
      if (!ConfigService.GroupConfig.IsValidGroup(group))
      {
        caller.SendErrorMessage($"Invalid group \"{group}\".");
        return;
      }

      caller.EnterPlayerTargetMode(selection => AddUserGroupToTarget(selection, group));
    }

    private void AddUserGroupToTarget(NwPlayerExtensions.PlayerTargetPlayerEvent selection, string group)
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

        if (entry.Groups.Contains(group))
        {
          caller.SendErrorMessage($"Target is already in group \"{group}\"");
          return;
        }

        entry.Groups ??= new List<string>();
        entry.Groups.Add(group);
        caller.SendErrorMessage($"Permission group granted: \"{group}\"");
      });
    }
  }
}
