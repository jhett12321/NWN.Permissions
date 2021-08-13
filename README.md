# NWN.Permissions
NWN.Permissions is a configurable and extensible permissions plugin for Anvil-based Neverwinter Nights servers that allows server admins to control abilities, commands and features for players and dungeon masters alike.

Out of the box, NWN.Permissions includes permission rules for controlling Dungeon Master powers in the DM client, and permissiosn for speaking in the shout channel, but this can be easily extended to support custom widgets and commands.

### Installation
1. Download the latest plugin from the Releases page: https://github.com/Jorteck/NWN.Permissions/releases
2. Unzip the file, and place the `NWN.Permissions` folder into your Anvil plugin directory. The directory structure should look like the following:
```
|----Plugins/
     |----NWN.Permissions/
          |----NWN.Permissions.dll
```
3. Launch the server. You should now be ready to start!

### Configuration
See the [Configuration Wiki Page](https://github.com/Jorteck/NWN.Permissions/wiki/Configuration-Reference) for a guide on configuring permissions in-game, or via the config .yml files.

### Adding permissions to your own plugin
To start, you will need to add the NWN.Permissions package as a reference in NuGet. This can be done by navigating to your plugin, and entering the follow command:

```
dotnet add package NWN.Permissions -v <desired version>
```

Where `<desired version>` is the version of `NWN.Permissions` you would like to to use.

Once installed, you will have access to the NWN.Permissions APIs. NWN.Permissions has a very simple interface for querying player permissions.

Simply reference `Jorteck.Permissions.PermissionsService` in your service constructor, and you can check your custom permissions using `PermissionService.HasPermission()`:

```cs
using Anvil.API;
using Anvil.API.Events;
using Anvil.Services;
using Jorteck.Permissions;

[ServiceBinding(typeof(CoolCommandService))]
public sealed class CoolCommandService
{
  // The permission key to check.
  private const string MyPermissionKey = "server.usecoolcommand";

  private readonly PermissionsService permissionsService;

  public CoolCommandService(PermissionsService permissionsService)
  {
    this.permissionsService = permissionsService;
    NwModule.Instance.OnPlayerChat += OnPlayerChat;
  }

  private void OnPlayerChat(ModuleEvents.OnPlayerChat eventData)
  {
    // Check if the message sent was the cool command.
    if (eventData.Message == "/cool")
    {
      // Check to see if the sender of this message has permission to use the command.
      if (permissionsService.HasPermission(eventData.Sender, MyPermissionKey))
      {
        // Tell them they are cool!
        eventData.Sender.SendServerMessage("You are awesome!");
      }
    }
  }
}
```
