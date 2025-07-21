using EvoSC.Common.Database.Models.Player;
using EvoSC.Common.Database.Repository;
using EvoSC.Common.Interfaces.Database;
using EvoSC.Common.Interfaces.Models;
using EvoSC.Common.Interfaces.Services;
using EvoSC.Common.Services.Attributes;
using EvoSC.Common.Services.Models;
using EvoSC.Manialinks.Interfaces;
using EvoSC.Modules.Interfaces;
using EvoSC.Modules.Official.UiControlModule.Interfaces;
using LinqToDB;
using LinqToDB.Common;
using Microsoft.Extensions.Logging;

namespace EvoSC.Modules.Official.UiControlModule.Services;

[Service(LifeStyle = ServiceLifeStyle.Transient)]
public class UiControlService(
    IManialinkManager manialinkManager,
    IModuleManager moduleManager,
    IServiceContainerManager serviceContainerManager,
    IDbConnectionFactory dbConnFactory,
    ILogger<UiControlService> logger
) : DbRepository(dbConnFactory), IUiControlService
{
    private const string ConfigMenuTemplate = "UiControlModule.Menu";

    public async Task DisplayMenuAsync(IOnlinePlayer player)
    {
        //TODO: Get available modules.
        //TODO: Get settings of user.

        var hiddenModules = new List<string>();

        if (player.Settings.HiddenManialinks != null)
        {
            hiddenModules.AddRange(player.Settings.HiddenManialinks.Split(", "));
        }

        var modulesThatUseManialinkManager = moduleManager.LoadedModules
            // .Where(moduleLoadContext =>
            // {
            //     try
            //     {
            //         return moduleLoadContext.Services.GetAllInstances(manialinkManager.GetType()).Any();
            //     }
            //     catch (ActivationException e)
            //     {
            //         logger.LogWarning("Couldn't detect manialink manager for {module}.",
            //             moduleLoadContext.ModuleInfo.Name);
            //
            //         return false;
            //     }
            // })
            .Select(moduleLoadContext => moduleLoadContext.ModuleInfo.Title)
            .ToList();

        logger.LogInformation("Modules: {services}.", modulesThatUseManialinkManager);
        logger.LogInformation("Hidden: {services}.", hiddenModules);

        await manialinkManager.SendManialinkAsync(player, ConfigMenuTemplate,
            new { hiddenModules, moduleNames = modulesThatUseManialinkManager });
    }

    public async Task SaveSettingsAsync(IOnlinePlayer player, List<string> hiddenManialinks)
    {
        string? hiddenManialinksString = null;

        if (!hiddenManialinks.IsNullOrEmpty())
        {
            hiddenManialinksString = string.Join(", ", hiddenManialinks);
        }

        player.Settings.HiddenManialinks = hiddenManialinksString;

        await Table<DbPlayerSettings>()
            .Where(dbPlayer => dbPlayer.PlayerId == player.Id)
            .Set(settings => settings.HiddenManialinks, hiddenManialinksString)
            .UpdateAsync();

        await manialinkManager.HideManialinkAsync(player, ConfigMenuTemplate);
    }
}
