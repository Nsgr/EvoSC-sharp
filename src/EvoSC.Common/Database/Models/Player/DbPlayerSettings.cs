using EvoSC.Common.Interfaces.Models;
using LinqToDB.Common;
using LinqToDB.Mapping;

namespace EvoSC.Common.Database.Models.Player;

[Table(TableName)]
public class DbPlayerSettings : IPlayerSettings
{
    public const string TableName = "PlayerSettings";

    [Column] public long PlayerId { get; set; }

    [Column] public string DisplayLanguage { get; set; }

    [Column] public string? HiddenManialinks { get; set; }

    public List<string> GetHiddenManialinks()
    {
        return HiddenManialinks != null
            ? HiddenManialinks.Split(",").ToList()
            : [];
    }

    public void SetHiddenManialinks(List<string> hiddenManialinks)
    {
        if (hiddenManialinks.IsNullOrEmpty())
        {
            HiddenManialinks = null;
            return;
        }

        HiddenManialinks = string.Join(",", hiddenManialinks);
    }
}
