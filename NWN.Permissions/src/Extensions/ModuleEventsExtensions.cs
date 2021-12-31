using System;
using Anvil.API;
using Anvil.API.Events;
using Jorteck.ChatTools;

namespace Jorteck.Permissions
{
  internal static class ModuleEventsExtensions
  {
    internal static void ProcessOnValidPCTargetBy(
        this ModuleEvents.OnPlayerTarget eventData,
        NwPlayer caller,
        Action<NwPlayer, NwPlayer> callback)
    {
      if (eventData.IsCancelled)
      {
        return;
      }

      if (!(eventData.TargetObject is NwCreature targetCreature &&
            targetCreature.IsLoginPlayerCharacter(out NwPlayer targetPlayer)))
      {
        eventData.Player.SendErrorMessage("Target must be a player character.");
        return;
      }

      caller.SendServerMessage($"Target: {targetCreature.GetObjectNameWithAccountNameAndCDKey()}", ColorConstants.Pink);
      callback(targetPlayer, caller);
    }

    private static string GetObjectNameWithAccountNameAndCDKey(this NwObject gameObject)
    {
      if (gameObject.IsPlayerControlled(out NwPlayer player))
      {
        return $"{gameObject.Name.StripColors()} ({player.PlayerName}, {player.CDKey})";
      }
      return $"{gameObject.Name.StripColors()}";
    }
  }
}
