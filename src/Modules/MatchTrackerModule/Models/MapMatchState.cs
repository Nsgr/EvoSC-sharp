using EvoSC.Modules.Official.MatchTrackerModule.Interfaces.Models;

namespace EvoSC.Modules.Official.MatchTrackerModule.Models;

public class MapMatchState : IMapMatchState
{
    public required Guid TimelineId { get; init; }
    public required MatchStatus Status { get; init; }
    public required DateTime Timestamp { get; init; }
    public required string MapUid { get; init; }
}
