using System;
using System.Collections.Generic;
using System.Text;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(HelpCommand))]
  [ServiceBinding(typeof(ICommand))]
  internal class HelpCommand : ICommand
  {
    [Inject]
    private PermissionsService PermissionsService { get; init; }

    [Inject]
    private PermissionsConfigService ConfigService { get; init; }

    [Inject]
    private Lazy<IReadOnlyCollection<ICommand>> Commands { get; init; }

    public string Command { get; }
    public string Permission { get; }
    public int? ArgCount { get; }
    public string Description { get; }
    public CommandUsage[] Usages { get; }

    private readonly StringBuilder stringBuilder = new StringBuilder();

    public HelpCommand()
    {
      Command = "help";
      Permission = PermissionConstants.Help;
      ArgCount = null;
      Description = "Shows this command list, or help for a specific command.";
      Usages = new[]
      {
        new CommandUsage("Show a list of all available commands."),
        new CommandUsage("<command>", "Show help for a specific command."),
      };
    }

    public void ProcessCommand(NwPlayer caller, IReadOnlyList<string> args)
    {
      if (args.Count == 0)
      {
        ShowAvailableCommandsToPlayer(caller);
      }
      else
      {
        string specifiedCommand = string.Join(" ", args);
        ICommand command = GetCommand(specifiedCommand);

        if (command != null)
        {
          ShowCommandHelpToPlayer(caller, command);
        }
        else
        {
          caller.SendServerMessage($"Unknown Command {specifiedCommand}.", ColorConstants.Red);
        }
      }
    }

    private IEnumerable<ICommand> GetAvailableCommands(NwPlayer player)
    {
      foreach (ICommand command in Commands.Value)
      {
        if (PermissionsService.HasPermission(player, command.Permission))
        {
          yield return command;
        }
      }
    }

    private ICommand GetCommand(string commandName)
    {
      foreach (ICommand command in Commands.Value)
      {
        if (command.Command == commandName)
        {
          return command;
        }
      }

      return null;
    }

    private void ShowAvailableCommandsToPlayer(NwPlayer player)
    {
      IEnumerable<ICommand> availableCommands = GetAvailableCommands(player);
      string message = GetCommandHelp(availableCommands);
      player.SendServerMessage(message, ColorConstants.White);
    }

    private void ShowCommandHelpToPlayer(NwPlayer player, ICommand command)
    {
      string message = GetCommandHelp(command);
      player.SendServerMessage(message, ColorConstants.White);
    }

    private string GetCommandHelp(ICommand command)
    {
      try
      {
        string formattedBaseCommand = $"{ConfigService.Config.ChatCommand} {command.Command}".ColorString(ColorConstants.Orange);
        stringBuilder.AppendLine("=================");
        stringBuilder.Append(formattedBaseCommand);
        stringBuilder.Append(": ");
        stringBuilder.AppendLine(command.Description);
        stringBuilder.AppendLine();

        stringBuilder.AppendLine("==Usages==");
        foreach (CommandUsage usage in command.Usages)
        {
          stringBuilder.Append(formattedBaseCommand);
          if (!string.IsNullOrEmpty(usage.SubCommand))
          {
            stringBuilder.Append(' ');
            stringBuilder.Append(usage.SubCommand.ColorString(ColorConstants.Orange));
          }

          stringBuilder.Append(": ");
          stringBuilder.AppendLine(usage.Description);
        }

        stringBuilder.AppendLine("=================");
        return stringBuilder.ToString().ColorString(ColorConstants.White);
      }
      finally
      {
        stringBuilder.Clear();
      }
    }

    private string GetCommandHelp(IEnumerable<ICommand> commands)
    {
      try
      {
        stringBuilder.AppendLine("=================");
        stringBuilder.AppendLine($"Use \"{ConfigService.Config.ChatCommand} help <command>\" for help on a specific command.".ColorString(ColorConstants.Silver));

        foreach (ICommand command in commands)
        {
          stringBuilder.Append($"{ConfigService.Config.ChatCommand} {command.Command}".ColorString(ColorConstants.Orange));
          stringBuilder.Append(": ");
          stringBuilder.AppendLine(command.Description);
        }

        stringBuilder.AppendLine("=================");
        return stringBuilder.ToString().ColorString(ColorConstants.White);
      }
      finally
      {
        stringBuilder.Clear();
      }
    }
  }
}
