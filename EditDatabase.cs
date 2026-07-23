using SPTarkov.DI.Annotations;
using SPTarkov.Server.Core.DI;
using SPTarkov.Server.Core.Helpers;
using SPTarkov.Server.Core.Models.Spt.Mod;
using SPTarkov.Server.Core.Models.Utils;
using SPTarkov.Server.Core.Services;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace IncreaseClimbHeight;

public record ModMetadata : AbstractModMetadata
{
public override string ModGuid { get; init; } = "redlaser42.IncreaseClimbHeight";
public override string Name { get; init; } = "Increase Climb Height";
public override string Author { get; init; } = "redlaser42";
public override List<string>? Contributors { get; init; }
public override SemanticVersioning.Version Version { get; init; } = new("2.0.1");
public override SemanticVersioning.Range SptVersion { get; init; } = new("~4.0.0");
public override List<string>? Incompatibilities { get; init; }
public override Dictionary<string, SemanticVersioning.Range>? ModDependencies { get; init; }
public override string? Url { get; init; }
public override bool? IsBundleMod { get; init; }
public override string License { get; init; } = "MIT";
}
public class VaultingConfig
{
    public SPTarkov.Server.Core.Models.Eft.Common.VaultingSettings VaultingSettings { get; set; } = new();
}

[Injectable(TypePriority = OnLoadOrder.PostDBModLoader + 1)]
public class EditDatabaseValues(DatabaseService databaseService, ModHelper modHelper)
    : IOnLoad
{
    public Task OnLoad()
    {
        VaultingConfig config = JsonSerializer.Deserialize<VaultingConfig>(File.ReadAllText(Path.Combine(modHelper.GetAbsolutePathToModFolder(Assembly.GetExecutingAssembly()), "config.json")))!;

        databaseService.GetGlobals().Configuration.VaultingSettings = config.VaultingSettings;

        return Task.CompletedTask;
    }
}