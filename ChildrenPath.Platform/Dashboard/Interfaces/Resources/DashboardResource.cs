namespace ChildrenPath.Platform.Dashboard.Interfaces.Resources;

public record DashboardResource(
    string Role,
    string TitleKey,
    string SubtitleKey,
    string HeroValue,
    string HeroLabelKey,
    IReadOnlyList<DashboardMetricResource> Metrics,
    IReadOnlyList<DashboardVehicleResource> Vehicles,
    IReadOnlyList<DashboardAlertResource> Alerts,
    IReadOnlyList<DashboardOperationItemResource> OperationItems,
    IReadOnlyList<DashboardActivityResource> Activities);

public record DashboardMetricResource(
    string Id, string Icon, string Value, string LabelKey, string HelperKey,
    string? Trend, string? TrendDirection);

public record DashboardVehicleResource(
    string Id, string Plate, string DriverName, double Lat, double Lng,
    string Status, string Route, int Students, int EtaMinutes, string LastUpdate);

public record DashboardAlertResource(
    int Id, string Type, string Priority, string Title, string Message,
    string Timestamp, bool Read, string RelatedVehicle);

public record DashboardOperationItemResource(
    string Id, string Label, string Value, string Helper, double Progress, string Icon);

public record DashboardActivityResource(
    string Id, string Time, string Title, string Description, string Status);