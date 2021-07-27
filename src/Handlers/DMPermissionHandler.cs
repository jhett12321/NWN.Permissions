using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;

namespace Jorteck.Permissions
{
  [ServiceBinding(typeof(DMPermissionHandler))]
  internal class DMPermissionHandler
  {
    private readonly PermissionsService permissionsService;

    public DMPermissionHandler(PermissionsService permissionsService)
    {
      this.permissionsService = permissionsService;

      NwModule module = NwModule.Instance;
      module.OnDMGiveXP += eventData => OnDMGive(eventData, PermissionConstants.DMGiveXp);
      module.OnDMGiveLevel += eventData => OnDMGive(eventData, PermissionConstants.DMGiveLevel);
      module.OnDMGiveGold += eventData => OnDMGive(eventData, PermissionConstants.DMGiveGold);
      module.OnDMGiveAlignment += OnDMGiveAlignment;
      module.OnDMGiveItemBefore += OnDMGiveItemBefore;

      module.OnDMPlayerDMLogin += OnPlayerDMLogin;
      module.OnDMPlayerDMLogout += OnPlayerDMLogout;
      module.OnClientEnter += OnClientEnter;

      module.OnDMHeal += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMHeal);
      module.OnDMKill += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMKill);
      module.OnDMToggleInvulnerable += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMInvulnerable);
      module.OnDMForceRest += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMForceRest);
      module.OnDMToggleImmortal += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMImmortal);
      module.OnDMLimbo += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMLimbo);
      module.OnDMToggleAI += eventData => OnDMGroupTarget(eventData, PermissionConstants.DMToggleAI);

      module.OnDMGoTo += eventData => OnDMSingleTarget(eventData, PermissionConstants.DMGoTo);
      module.OnDMPossess += eventData => OnDMSingleTarget(eventData, PermissionConstants.DMPossess);
      module.OnDMPossessFullPower += eventData => OnDMSingleTarget(eventData, PermissionConstants.DMPossessFullPower);
      module.OnDMToggleLock += eventData => OnDMSingleTarget(eventData, PermissionConstants.DMToggleLock);
      module.OnDMDisableTrap += eventData => OnDMSingleTarget(eventData, PermissionConstants.DMDisableTrap);

      module.OnDMAppear += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMAppear);
      module.OnDMDisappear += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMDisappear);
      module.OnDMSetFaction += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMSetFaction);
      module.OnDMGetFactionReputation += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMGetFactionReputation);
      module.OnDMSetFactionReputation += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMSetFactionReputation);
      module.OnDMTakeItem += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMTakeItem);
      module.OnDMSetStat += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMSetStat);
      module.OnDMSetTime += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMSetTime);
      module.OnDMSetDate += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMSetDate);
      module.OnDMGetVariable += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMGetLocal);
      module.OnDMSetVariable += eventData => OnDMStandardEvent(eventData, PermissionConstants.DMSetLocal);
      module.OnDMDumpLocals += OnDMDumpLocals;

      module.OnDMJumpToPoint += eventData => OnDMTeleport(eventData, PermissionConstants.DMJump + PermissionConstants.TargetSelf);
      module.OnDMJumpAllPlayersToPoint += eventData => OnDMTeleport(eventData, PermissionConstants.DMJumpAllPlayers);
      module.OnDMJumpTargetToPoint += OnDMJumpTargetToPoint;

      module.OnDMChangeDifficulty += OnDMChangeDifficulty;
      module.OnDMViewInventory += OnDMViewInventory;

      module.OnDMSpawnObjectBefore += OnDMSpawnObject;
    }

    private void OnClientEnter(ModuleEvents.OnClientEnter eventData)
    {
      if (permissionsService.HasPermission(eventData.Player, PermissionConstants.PlayerDMForceLogin))
      {
        eventData.Player.IsPlayerDM = true;
      }
    }

    private void OnPlayerDMLogin(OnDMPlayerDMLogin eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.PlayerDMLogin))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnPlayerDMLogout(OnDMPlayerDMLogout eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.PlayerDMLogout))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGiveItemBefore(OnDMGiveItemBefore eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMGiveItem))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, PermissionConstants.DMGiveItem))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMDumpLocals(OnDMDumpLocals eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMDumpLocals))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, PermissionConstants.DMDumpLocals))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGive(DMGiveEvent eventData, string permission)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGiveAlignment(OnDMGiveAlignment eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMGiveAlignment))
      {
        return;
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, PermissionConstants.DMGiveAlignment))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMGroupTarget(DMGroupTargetEvent eventData, string permissionBase)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, permissionBase))
      {
        return;
      }

      foreach (NwObject target in eventData.Targets)
      {
        if (!HasPermissionToTarget(eventData.DungeonMaster, target, permissionBase))
        {
          eventData.Skip = true;
          ShowNoPermissionError(eventData.DungeonMaster);
          break;
        }
      }
    }

    private void OnDMSingleTarget(DMSingleTargetEvent eventData, string permissionBase)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, permissionBase))
      {
        return;
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, permissionBase))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMStandardEvent(DMStandardEvent eventData, string permission)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMTeleport(DMTeleportEvent eventData, string permission)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, permission))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMJumpTargetToPoint(OnDMJumpTargetToPoint eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMJump))
      {
        return;
      }

      foreach (NwGameObject target in eventData.Targets)
      {
        if (!HasPermissionToTarget(eventData.DungeonMaster, target, PermissionConstants.DMJump))
        {
          eventData.Skip = true;
          ShowNoPermissionError(eventData.DungeonMaster);
          break;
        }
      }
    }

    private void OnDMChangeDifficulty(OnDMChangeDifficulty eventData)
    {
      if (!permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMChangeDifficulty))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMViewInventory(OnDMViewInventory eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMViewInventory))
      {
        return;
      }

      if (!HasPermissionToTarget(eventData.DungeonMaster, eventData.Target, PermissionConstants.DMViewInventory))
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private void OnDMSpawnObject(OnDMSpawnObjectBefore eventData)
    {
      if (permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn))
      {
        return;
      }

      bool hasPermission = eventData.ObjectType switch
      {
        ObjectTypes.Creature => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetCreature),
        ObjectTypes.Item => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetItem),
        ObjectTypes.Trigger => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetTrigger),
        ObjectTypes.Door => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetDoor),
        ObjectTypes.Waypoint => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetWaypoint),
        ObjectTypes.Placeable => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetPlaceable),
        ObjectTypes.Store => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetStore),
        ObjectTypes.Encounter => permissionsService.HasPermission(eventData.DungeonMaster, PermissionConstants.DMSpawn + PermissionConstants.TargetEncounter),
        _ => false,
      };

      if (!hasPermission)
      {
        eventData.Skip = true;
        ShowNoPermissionError(eventData.DungeonMaster);
      }
    }

    private bool HasPermissionToTarget(NwPlayer dungeonMaster, NwObject target, string permissionBase)
    {
      bool targetSelf = permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetSelf);
      bool targetPlayer = permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetPlayer);

      NwCreature dmCreature = dungeonMaster.ControlledCreature;

      if (targetSelf && dmCreature == target)
      {
        return true;
      }

      if (targetPlayer && target != dmCreature && (target.IsPlayerControlled(out _) || target.IsLoginPlayerCharacter(out _)))
      {
        return true;
      }

      return target switch
      {
        NwCreature => permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetCreature),
        NwItem => permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetItem),
        NwEncounter => permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetEncounter),
        NwWaypoint => permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetWaypoint),
        NwTrigger => permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetTrigger),
        NwPlaceable => permissionsService.HasPermission(dungeonMaster, permissionBase + PermissionConstants.TargetPlaceable),
        _ => false,
      };
    }

    private void ShowNoPermissionError(NwPlayer player)
    {
      player.SendServerMessage("You do not have permission to do that.", ColorConstants.Red);
    }
  }
}
