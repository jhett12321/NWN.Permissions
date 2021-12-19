using System.Collections.Generic;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(ICommand))]
  internal class UserAddGroupCommand : ICommand
  {
    private readonly PermissionsConfigService permissionsConfigService;

    public string Command { get; }
    public string Permission { get; }
    public int? ArgCount { get; }
    public string Description { get; }
    public CommandUsage[] Usages { get; }

    public UserAddGroupCommand(PermissionsConfigService permissionsConfigService)
    {
      this.permissionsConfigService = permissionsConfigService;

      Command = "user addgroup";
      Permission = PermissionConstants.UserAddGroup;
      ArgCount = 1;
      Description = "Adds a group membership to a user.";
      Usages = new[]
      {
        new CommandUsage("<group_name>", "Add a player to the specified group."),
      };
    }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      string group = args[0];
      if (!permissionsConfigService.GroupConfig.IsValidGroup(group))
      {
        caller.SendErrorMessage($"Invalid group \"{group}\".");
        return;
      }

      caller.EnterTargetMode(OnCreatureSelection);

      void OnCreatureSelection(ModuleEvents.OnPlayerTarget eventData)
      {
        if (eventData.IsCancelled || eventData.TargetObject.IsLoginPlayerCharacter(out NwPlayer player))
        {
          return;
        }

        permissionsConfigService.UpdateUserConfig(config =>
        {
          if (!config.UsersCd.TryGetValue(player.CDKey, out UserEntry entry))
          {
            entry = new UserEntry();
            config.UsersCd[player.CDKey] = entry;
          }

          if (entry.Groups.Contains(group))
          {
            caller.SendErrorMessage($"{player.PlayerName} is already in group \"{group}\"");
            return;
          }

          entry.Groups ??= new List<string>();
          entry.Groups.Add(group);
          caller.SendErrorMessage($"Added {player.PlayerName} to group \"{group}\"");
        });
      }
    }
  }
}

