using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(ChatHandler))]
  internal sealed class ChatHandler
  {
    private readonly PermissionsService permissionsService;

    public ChatHandler(PermissionsService permissionsService)
    {
      this.permissionsService = permissionsService;

      NwModule.Instance.OnChatMessageSend += OnChatMessageSend;
    }

    private void OnChatMessageSend(OnChatMessageSend eventData)
    {
      if (!eventData.Sender.IsPlayerControlled(out NwPlayer player))
      {
        return;
      }

      if (eventData.ChatChannel == ChatChannel.PlayerShout || eventData.ChatChannel == ChatChannel.DmShout)
      {
        if (!permissionsService.HasPermission(player, PermissionConstants.ChatShout))
        {
          eventData.Skip = true;
          ShowNoPermissionError(player);
        }
      }
    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendServerMessage("You do not have permission to shout.", ColorConstants.Red);
    }
  }
}
