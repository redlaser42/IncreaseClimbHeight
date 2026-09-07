using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers.Server;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Spt.Tables;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace IncreaseClimbHeight;

public record ModMetadata : IModMetadata
{
    public string ModGuid { get; init; } = "redlaser42.IncreaseClimbHeight";
    public string Name { get; init; } = "Increase Climb Height";
    public string Author { get; init; } = "redlaser42";
    public List<string>? Contributors { get; init; }
    public SemanticVersioning.Version Version { get; init; } = new("2.1.1");
    public SemanticVersioning.Range SptVersion { get; init; } = new("~4.1.0");
    public bool HasPrepatcher { get; init; }
    public List<string>? Incompatibilities { get; init; }
    public Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
    public string? Url { get; init; }
    public string License { get; init; } = "MIT";
}

public class VaultingConfig
{
    public VaultingSettings VaultingSettings { get; set; } = new();
}

[Injectable(InjectionType.Singleton, int.MaxValue, TypePriority = 250000)]
public class EditDatabaseValues(GlobalTable globals, ModHelper modHelper)
    : IOnLoad
{
    public Task OnLoadAsync(CancellationToken cancellationToken)
    {
        VaultingConfig config = JsonSerializer.Deserialize<VaultingConfig>(File.ReadAllText(Path.Combine(modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly()), "config.json")))!;

        globals.Configuration.VaultingSettings = config.VaultingSettings;

        return Task.CompletedTask;
    }
}
