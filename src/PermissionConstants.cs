namespace Jorteck.Permissions
{
  public static class PermissionConstants
  {
    // Target suffixes
    public const string TargetSelf = ".self";
    public const string TargetCreature = ".creature";
    public const string TargetPlayer = ".player";
    public const string TargetItem = ".item";
    public const string TargetEncounter = ".encounter";
    public const string TargetWaypoint = ".waypoint";
    public const string TargetTrigger = ".trigger";
    public const string TargetDoor = ".door";
    public const string TargetPlaceable = ".placeable";
    public const string TargetStore = ".store";

    // Permission Management
    public const string UseCommand = "permissions.manage";
    public const string UseReloadCommand = "permissions.manage.reload";

    // Chat Permissions
    public const string ChatShout = "chat.shout";

    // Player DM
    public const string PlayerDMLogin = "playerdm.login";
    public const string PlayerDMForceLogin = "playerdm.forcelogin";
    public const string PlayerDMLogout = "playerdm.logout";

    // DM Give Powers
    public const string DMGiveGold = "dm.give.gold";
    public const string DMGiveXp = "dm.give.xp";
    public const string DMGiveLevel = "dm.give.level";
    public const string DMGiveAlignment = "dm.give.alignment";
    public const string DMGiveItem = "dm.give.item";

    // DM Spawn Powers
    public const string DMSpawn = "dm.spawn";

    // DM Multi Object Powers
    public const string DMHeal = "dm.heal";
    public const string DMKill = "dm.kill.self";
    public const string DMInvulnerable = "dm.invulnerable.self";
    public const string DMForceRest = "dm.forcerest.self";
    public const string DMImmortal = "dm.immortal.self";
    public const string DMLimbo = "dm.limbo";
    public const string DMToggleAI = "dm.toggleai";

    // DM Single Object Powers
    public const string DMGoTo = "dm.goto";
    public const string DMPossess = "dm.possess";
    public const string DMPossessFullPower = "dm.possess.full";
    public const string DMToggleLock = "dm.lock.toggle";
    public const string DMDisableTrap = "dm.trap.disable";

    // DM Misc Powers
    public const string DMJump = "dm.jump";
    public const string DMJumpAllPlayers = "dm.jump.allplayers";
    public const string DMChangeDifficulty = "dm.changedifficulty";
    public const string DMViewInventory = "dm.viewinventory";
    public const string DMSpawnTrap = "dm.spawntrap";

    // DM Local Variables
    public const string DMGetLocal = "dm.local.get";
    public const string DMSetLocal= "dm.local.set";
    public const string DMDumpLocals = "dm.local.dump";

    // DM Commands
    public const string DMAppear = "dm.appear";
    public const string DMDisappear = "dm.disappear";
    public const string DMSetFaction = "dm.faction.set";
    public const string DMGetFactionReputation = "dm.faction.getreputation";
    public const string DMSetFactionReputation = "dm.faction.setreputation";
    public const string DMTakeItem = "dm.takeitem";
    public const string DMSetStat = "dm.setstat";
    public const string DMSetTime = "dm.time.settime";
    public const string DMSetDate = "dm.time.setdate";
  }
}
