using System;
using Anvil.API;

namespace Jorteck.Permissions
{
  internal class HelpCommand : ICommand
  {
    private readonly Lazy<PermissionsChatCommandService> chatCommandService;

    public string Command { get; }
    public string Permission { get; }
    public int? ArgCount { get; }
    public string Description { get; }
    public string[] Usages { get; }

    public HelpCommand(Lazy<PermissionsChatCommandService> chatCommandService)
    {
      this.chatCommandService = chatCommandService;

      Command = "help";
      Permission = PermissionConstants.Help;
      ArgCount = null;
      Description = "Shows this command list, or help for a specific command.";
      Usages = new[]
      {
        $"{Command}            Show a list of all available commands.",
        $"{Command} <command>  Show help for a specific command.",
      };
    }

    public void ProcessCommand(NwPlayer caller, string[] args)
    {
      string specifiedCommand = string.Join(" ", args);
      ICommand command = chatCommandService.Value.GetCommand(specifiedCommand);

      if (command == null)
      {
        caller.SendServerMessage($"Unknown Command {specifiedCommand}.", ColorConstants.Red);
      }

      if (args.Length == 0 || command == null)
      {
        chatCommandService.Value.ShowAvailableCommandsToPlayer(caller);
      }
      else
      {
        chatCommandService.Value.ShowCommandHelpToPlayer(caller, command);
      }
    }
  }
}
