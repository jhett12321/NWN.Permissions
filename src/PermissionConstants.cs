namespace Jorteck.Permissions
{
  public static class PermissionConstants
  {
    // Permission Management
    public static string UseCommand = "permissions.manage";
    public static string UseReloadCommand = "permissions.manage.reload";

    // Chat Permissions
    public static string Shout = "chat.shout";

    // Player DM
    public static string PlayerDMLogin = "playerdm.login";
    public static string PlayerDMLoginNoPassword = "playerdm.loginnopass";
    public static string PlayerDMForceLogin = "playerdm.forcelogin";
    public static string PlayerDMLogout = "playerdm.logout";

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

    // DM Multi Object Powers
    public static string DMHealSelf = "dm.heal.self";
    public static string DMHealCreature = "dm.heal.creature";
    public static string DMHealPlayer = "dm.heal.player";
    public static string DMKillSelf = "dm.kill.self";
    public static string DMKillCreature = "dm.kill.creature";
    public static string DMKillPlayer = "dm.kill.player";
    public static string DMInvulnerableSelf = "dm.invulnerable.self";
    public static string DMInvulnerableCreature = "dm.invulnerable.creature";
    public static string DMInvulnerablePlayer = "dm.invulnerable.player";
    public static string DMForceRestSelf = "dm.forcerest.self";
    public static string DMForceRestCreature = "dm.forcerest.creature";
    public static string DMForceRestPlayer = "dm.forcerest.player";
    public static string DMImmortalSelf = "dm.immortal.self";
    public static string DMImmortalCreature = "dm.immortal.creature";
    public static string DMImmortalPlayer = "dm.immortal.player";
    public static string DMLimbo = "dm.limbo";
    public static string DMToggleAI = "dm.toggleai";

    // DM Single Object Powers
    public static string DMGoToObject = "dm.goto.object";
    public static string DMGoToPlayer = "dm.goto.player";
    public static string DMGoToLocation = "dm.goto.location";
    public static string DMPossess = "dm.possess";
    public static string DMPossessFullPower = "dm.possess.full";
    public static string DMToggleLock = "dm.lock.toggle";
    public static string DMDisable = "dm.trap.disable";

    // DM Misc Powers
    public static string DMJumpCreatureToPoint = "dm.jump.creature";
    public static string DMJumpPlayerToPoint = "dm.jump.player";
    public static string DMJumpAllPlayersToPoint = "dm.jump.allplayers";
    public static string DMChangeDifficulty = "dm.changedifficulty";
    public static string DMViewInventoryCreature = "dm.viewinventory.creature";
    public static string DMViewInventoryPlayer = "dm.viewinventory.player";
    public static string DMSpawnTrap = "dm.spawntrap";

    // DM Local Variables
    public static string DMGetLocalCreature = "dm.local.get.creature";
    public static string DMGetLocalPlayer = "dm.local.get.player";
    public static string DMGetLocalItem = "dm.local.get.item";
    public static string DMGetLocalEncounter = "dm.local.get.encounter";
    public static string DMGetLocalWaypoint = "dm.local.get.waypoint";
    public static string DMGetLocalTrigger = "dm.local.get.trigger";
    public static string DMGetLocalPortal = "dm.local.get.portal";
    public static string DMGetLocalPlaceable = "dm.local.get.placeable";

    public static string DMSetLocalCreature = "dm.local.set.creature";
    public static string DMSetLocalPlayer = "dm.local.set.player";
    public static string DMSetLocalItem = "dm.local.set.item";
    public static string DMSetLocalEncounter = "dm.local.set.encounter";
    public static string DMSetLocalWaypoint = "dm.local.set.waypoint";
    public static string DMSetLocalTrigger = "dm.local.set.trigger";
    public static string DMSetLocalPortal = "dm.local.set.portal";
    public static string DMSetLocalPlaceable = "dm.local.set.placeable";

    public static string DMDumpLocalsCreature = "dm.local.dump.creature";
    public static string DMDumpLocalsPlayer = "dm.local.dump.player";
    public static string DMDumpLocalsItem = "dm.local.dump.item";
    public static string DMDumpLocalsEncounter = "dm.local.dump.encounter";
    public static string DMDumpLocalsWaypoint = "dm.local.dump.waypoint";
    public static string DMDumpLocalsTrigger = "dm.local.dump.trigger";
    public static string DMDumpLocalsPortal = "dm.local.dump.portal";
    public static string DMDumpLocalsPlaceable = "dm.local.dump.placeable";

    // DM Commands
    public static string DMAppear = "dm.appear";
    public static string DMDisappear = "dm.disappear";
    public static string DMSetFaction = "dm.faction.set";
    public static string DMGetFactionReputation = "dm.faction.getreputation";
    public static string DMSetFactionReputation = "dm.faction.setreputation";
    public static string DMTakeItemCreature = "dm.takeitem.creature";
    public static string DMTakeItemPlayer = "dm.takeitem.player";
    public static string DMSetStatCreature = "dm.setstat.creature";
    public static string DMSetStatPlayer = "dm.setstat.player";
    public static string DMSetTime = "dm.time.settime";
    public static string DMSetDate = "dm.time.setdate";
  }
}
