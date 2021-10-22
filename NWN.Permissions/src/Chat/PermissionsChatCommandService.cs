using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Permissions
{
  internal sealed class PermissionsChatCommandService
  {
    private readonly PermissionsService permissionsService;
    private readonly PermissionsConfigService configService;
    private readonly CommandHelpRenderer commandHelpRenderer;

    private readonly List<ICommand> commands;

    public PermissionsChatCommandService(PermissionsService permissionsService, PermissionsConfigService configService, CommandHelpRenderer commandHelpRenderer, IEnumerable<ICommand> commands)
    {
      if (!configService.Config.ChatCommandEnable)
      {
        return;
      }

      this.permissionsService = permissionsService;
      this.configService = configService;
      this.commandHelpRenderer = commandHelpRenderer;

      this.commands = commands.ToList();

      NwModule.Instance.OnChatMessageSend += OnChatMessageSend;
    }

    public IEnumerable<ICommand> GetAvailableCommands(NwPlayer player)
    {
      foreach (ICommand command in commands)
      {
        if (permissionsService.HasPermission(player, command.Permission))
        {
          yield return command;
        }
      }
    }

    public ICommand GetCommand(string commandName)
    {
      foreach (ICommand command in commands)
      {
        if (command.Command == commandName)
        {
          return command;
        }
      }

      return null;
    }

    public void ShowAvailableCommandsToPlayer(NwPlayer player)
    {
      IEnumerable<ICommand> availableCommands = GetAvailableCommands(player);
      string message = commandHelpRenderer.GetCommandHelp(availableCommands);
      player.SendServerMessage(message, ColorConstants.White);
    }

    public void ShowCommandHelpToPlayer(NwPlayer player, ICommand command)
    {
      string message = commandHelpRenderer.GetCommandHelp(command);
      player.SendServerMessage(message, ColorConstants.White);
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
        ShowAvailableCommandsToPlayer(player);
      }
      else if (eventData.Message.StartsWith(chatCommand + " "))
      {
        TryProcessCommand(player, eventData.Message[(chatCommand.Length + 1)..]);
      }
    }

    private void TryProcessCommand(NwPlayer sender, string rawCommand)
    {
      foreach (ICommand command in commands)
      {
        if (rawCommand == command.Command)
        {
          TryExecuteCommand(sender, command, ImmutableArray<string>.Empty);
        }
      }

      foreach (ICommand command in commands)
      {
        if (rawCommand.StartsWith(command.Command))
        {
          string[] args = GetArgs(rawCommand[command.Command.Length..]);
          TryExecuteCommand(sender, command, args);
        }
      }
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
        return;
      }

      if (command.ArgCount.HasValue && command.ArgCount != args.Count)
      {
        ShowCommandHelpToPlayer(sender, command);
      }

      command.ProcessCommand(sender, args);
    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendServerMessage("You do not have permission to do that.", ColorConstants.Red);
    }
  }
}
