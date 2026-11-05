namespace ChildrenPath.Platform.Dashboard.Interfaces.Resources;

public record DashboardResponse(
    string Role,
    string TitleKey,
    string SubtitleKey,
    string HeroValue,
    string HeroLabelKey,
    List<object> Metrics,
    List<object> Vehicles,
    List<object> Alerts,
    List<object> OperationItems,
    List<object> Activities
);