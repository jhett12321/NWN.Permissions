using System.Linq;
using System.Text.RegularExpressions;
using Anvil.API;
using Anvil.API.Events;

namespace Jorteck.Permissions
{
  internal class PermissionsChatCommandService
  {
    private readonly PermissionsService permissionsService;
    private readonly PermissionsConfigService configService;

    public PermissionsChatCommandService(PermissionsService permissionsService, PermissionsConfigService configService)
    {
      if (!configService.Config.ChatCommandEnable)
      {
        return;
      }

      this.permissionsService = permissionsService;
      this.configService = configService;

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
        if (permissionsService.HasPermission(player, PermissionConstants.Help))
        {
          ShowCommandHelp(player);
        }
        else
        {
          ShowNoPermissionError(player);
        }
      }
      else if (eventData.Message.StartsWith(chatCommand + " "))
      {
        TryProcessCommand(player, eventData.Message[(chatCommand.Length + 1)..]);
      }
    }

    private void TryProcessCommand(NwPlayer sender, string rawArgs)
    {
      string[] args = Regex.Matches(rawArgs, @"\""(\""\""|[^\""])+\""|[^ ]+", RegexOptions.ExplicitCapture)
        .Select(m => m.Value.StartsWith("\"") ? m.Value.Substring(1, m.Length - 2).Replace("\"\"", "\"") : m.Value)
        .ToArray();

      switch (args[0])
      {
        case "help":
          ShowCommandHelp(sender);
          break;
      }
    }

    private void ShowCommandHelp(NwPlayer sender)
    {

    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendServerMessage("You do not have permission to do that.", ColorConstants.Red);
    }
  }
}
