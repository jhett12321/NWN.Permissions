using Anvil.API;

namespace Jorteck.Permissions
{
  internal static class PlayerExtensions
  {
    public static void SendErrorMessage(this NwPlayer player, string message)
    {
      player.SendServerMessage(message, ColorConstants.Red);
    }
  }
}
