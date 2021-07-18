using Anvil.API;
using Anvil.Services;
using NLog;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(PermissionsService))]
  public sealed class PermissionsService
  {
    private static readonly Logger Log = LogManager.GetCurrentClassLogger();

    private readonly PermissionsConfigService permissionsConfigService;

    public PermissionsService(PermissionsConfigService permissionsConfigService)
    {
      this.permissionsConfigService = permissionsConfigService;
    }

    /// <summary>
    /// Gets if a player has the specified permission.
    /// </summary>
    /// <param name="player">The player to check.</param>
    /// <param name="permission">The permission to query.</param>
    /// <returns>True if the player has the specified permission, otherwise false.</returns>
    public bool HasPermission(NwPlayer player, string permission)
    {
      PermissionSet permissionSet = permissionsConfigService.GetPermissionsForPlayer(player);
      return permissionSet.Permissions.Contains(permission);
    }
  }
}
