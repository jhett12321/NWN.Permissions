using System;
using System.Collections.Generic;
using System.Text;
using Anvil.API;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(CommandHelpRenderer))]
  internal sealed class CommandHelpRenderer
  {
    private readonly PermissionsConfigService configService;
    private readonly StringBuilder stringBuilder = new StringBuilder();

    public CommandHelpRenderer(PermissionsConfigService configService)
    {
      this.configService = configService;
    }

    public string GetCommandHelp(ICommand command)
    {
      try
      {
        stringBuilder.AppendLine("=================");
        stringBuilder.Append($"{configService.Config.ChatCommand} {command.Command}".ColorString(ColorConstants.Orange));
        stringBuilder.Append(": ");
        stringBuilder.AppendLine(command.Description);
        stringBuilder.AppendLine();

        stringBuilder.AppendLine("Usages: ");
        foreach (string usage in command.Usages)
        {
          stringBuilder.AppendLine(usage);
        }

        stringBuilder.AppendLine("=================");
        return stringBuilder.ToString().ColorString(ColorConstants.White);
      }
      finally
      {
        stringBuilder.Clear();
      }
    }

    public string GetCommandHelp(IEnumerable<ICommand> commands)
    {
      try
      {
        stringBuilder.AppendLine("=================");
        stringBuilder.AppendLine($"Use \"{configService.Config.ChatCommand} help <command>\" for help on a specific command.".ColorString(ColorConstants.Gray));

        foreach (ICommand command in commands)
        {
          stringBuilder.Append($"{configService.Config.ChatCommand} {command.Command}".ColorString(ColorConstants.Orange));
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
