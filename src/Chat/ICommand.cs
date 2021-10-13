using Anvil.API;

namespace Jorteck.Permissions
{
  internal interface ICommand
  {
    /// <summary>
    /// Gets the default name for this command.
    /// </summary>
    string Command { get; }

    /// <summary>
    /// Gets the permission key required to use this command.
    /// </summary>
    string Permission { get; }

    /// <summary>
    /// Gets the number of arguments expected by this command, otherwise null if this can be any.
    /// </summary>
    int? ArgCount { get; }

    /// <summary>
    /// Gets the description for this command.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets a list of example usages for this command.
    /// </summary>
    string[] Usages { get; }

    /// <summary>
    /// Process this command with the specified arguments.
    /// </summary>
    /// <param name="caller">The calling player of this command.</param>
    /// <param name="args">Any additional arguments specified.</param>
    void ProcessCommand(NwPlayer caller, string[] args);
  }
}
