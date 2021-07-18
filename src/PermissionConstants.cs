namespace Jorteck.Permissions
{
  public static class PermissionConstants
  {
    // Permission Management
    public static string UseCommand = "permissions.manage";
    public static string UseReloadCommand = "permissions.manage.reload";

    // Player DM
    public static string PlayerDMLogin = "playerdm.login";
    public static string PlayerDMLoginNoPassword = "playerdm.loginnopass";
    public static string PlayerDMForceLogin = "playerdm.forcelogin";

    // DM Give Powers
    public static string DMGiveGold = "dm.give.gold";
    public static string DMGiveXp = "dm.give.xp";
    public static string DMGiveLevel = "dm.give.level";
    public static string DMGiveAlignment = "dm.give.alignment";
    public static string DMGiveItem = "dm.give.item";

    // DM Spawn Powers
    public static string DMSpawnCreature = "dm.spawn.creature";
    public static string DMSpawnItem = "dm.spawn.item";
    public static string DMSpawnEncounter = "dm.spawn.encounter";
    public static string DMSpawnWaypoint = "dm.spawn.waypoint";
    public static string DMSpawnTrigger = "dm.spawn.trigger";
    public static string DMSpawnPortal = "dm.spawn.portal";
    public static string DMSpawnPlaceable = "dm.spawn.placeable";


  }
}
