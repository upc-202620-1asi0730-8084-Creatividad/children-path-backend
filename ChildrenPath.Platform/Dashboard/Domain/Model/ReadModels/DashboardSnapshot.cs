using ChildrenPath.Platform.Dashboard.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Dashboard.Domain.Model.ReadModels;

public record DashboardSnapshot(
    DashboardType Type,
    string HeroValue,
    IReadOnlyList<DashboardMetricData> Metrics,
    IReadOnlyList<DashboardVehicleData> Vehicles,
    IReadOnlyList<DashboardAlertData> Alerts,
    IReadOnlyList<DashboardOperationItemData> OperationItems,
    IReadOnlyList<DashboardActivityData> Activities);

public record DashboardMetricData(
    string Id, string Icon, string Value, string LabelKey, string HelperKey,
    string? Trend, string? TrendDirection);

public record DashboardVehicleData(
    string Id, string Plate, string DriverName, double Lat, double Lng,
    string Status, string Route, int Students, int EtaMinutes, string LastUpdate);

public record DashboardAlertData(
    int Id, string Type, string Priority, string Title, string Message,
    string Timestamp, bool Read, string RelatedVehicle);

public record DashboardOperationItemData(
    string Id, string Label, string Value, string Helper, double Progress, string Icon);

public record DashboardActivityData(
    string Id, string Time, string Title, string Description, string Status);