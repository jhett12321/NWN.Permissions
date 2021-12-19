using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(PermissionsChatCommandService))]
  internal sealed class PermissionsChatCommandService
  {
    private readonly PermissionsService permissionsService;
    private readonly PermissionsConfigService configService;

    private readonly List<ICommand> commands;
    private readonly HelpCommand helpCommand;
    private readonly string helpCommandText;

    public PermissionsChatCommandService(PermissionsService permissionsService, PermissionsConfigService configService, IEnumerable<ICommand> commands, HelpCommand helpCommand)
    {
      if (!configService.Config.ChatCommandEnable)
      {
        return;
      }

      this.permissionsService = permissionsService;
      this.configService = configService;
      this.commands = commands.ToList();
      this.helpCommand = helpCommand;
      helpCommandText = $"{configService.Config.ChatCommand} {helpCommand.Command}".ColorString(ColorConstants.Orange);

      NwModule.Instance.OnChatMessageSend += OnChatMessageSend;
    }

    private void OnChatMessageSend(OnChatMessageSend eventData)
    {
      if (!eventData.Sender.IsPlayerControlled(out NwPlayer player))
      {
        return;
      }

      string chatCommand = configService.Config.ChatCommand;

      if (eventData.Message == chatCommand)
      {
        TryExecuteCommand(player, helpCommand, ImmutableArray<string>.Empty);
        eventData.Skip = true;
      }
      else if (eventData.Message.StartsWith(chatCommand + " "))
      {
        bool validCommand = TryProcessCommand(player, eventData.Message[(chatCommand.Length + 1)..]);
        eventData.Skip = true;

        if (!validCommand)
        {
          player.SendErrorMessage($"Unknown command. Type {helpCommandText} for help.");
        }
      }
    }

    private bool TryProcessCommand(NwPlayer sender, string rawCommand)
    {
      foreach (ICommand command in commands)
      {
        if (rawCommand == command.Command)
        {
          TryExecuteCommand(sender, command, ImmutableArray<string>.Empty);
          return true;
        }
      }

      foreach (ICommand command in commands)
      {
        if (rawCommand.StartsWith(command.Command))
        {
          string[] args = GetArgs(rawCommand[command.Command.Length..]);
          TryExecuteCommand(sender, command, args);
          return true;
        }
      }

      return false;
    }

    private string[] GetArgs(string rawArgs)
    {
      return Regex.Matches(rawArgs, @"\""(\""\""|[^\""])+\""|[^ ]+", RegexOptions.ExplicitCapture)
        .Select(m => m.Value.StartsWith("\"") ? m.Value.Substring(1, m.Length - 2).Replace("\"\"", "\"") : m.Value)
        .ToArray();
    }

    private void TryExecuteCommand(NwPlayer sender, ICommand command, IReadOnlyList<string> args)
    {
      if (!permissionsService.HasPermission(sender, command.Permission))
      {
        ShowNoPermissionError(sender);
      }
      else if (command.ArgCount.HasValue && command.ArgCount != args.Count)
      {
        TryExecuteCommand(sender, helpCommand, new[] { command.Command });
      }
      else
      {
        command.ProcessCommand(sender, args);
      }
    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendServerMessage("You do not have permission to do that.", ColorConstants.Red);
    }
  }
}
