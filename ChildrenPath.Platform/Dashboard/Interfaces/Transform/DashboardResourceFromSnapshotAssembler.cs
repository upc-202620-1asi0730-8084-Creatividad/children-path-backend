using ChildrenPath.Platform.Dashboard.Domain.Model.ReadModels;
using ChildrenPath.Platform.Dashboard.Interfaces.Resources;

namespace ChildrenPath.Platform.Dashboard.Interfaces.Transform;

public static class DashboardResourceFromSnapshotAssembler
{
    public static DashboardResource ToResourceFromSnapshot(DashboardSnapshot s)
    {
        var role = s.Type.ToString().ToLowerInvariant();

        return new DashboardResource(
            Role: role,
            TitleKey: $"dashboardPage.roles.{role}.title",
            SubtitleKey: $"dashboardPage.roles.{role}.subtitle",
            HeroValue: s.HeroValue,
            HeroLabelKey: $"dashboardPage.hero.{role}",
            Metrics: s.Metrics.Select(m =>
                new DashboardMetricResource(m.Id, m.Icon, m.Value, m.LabelKey, m.HelperKey, m.Trend, m.TrendDirection)).ToList(),
            Vehicles: s.Vehicles.Select(v =>
                new DashboardVehicleResource(v.Id, v.Plate, v.DriverName, v.Lat, v.Lng, v.Status, v.Route,
                    v.Students, v.EtaMinutes, v.LastUpdate)).ToList(),
            Alerts: s.Alerts.Select(a =>
                new DashboardAlertResource(a.Id, a.Type, a.Priority, a.Title, a.Message, a.Timestamp,
                    a.Read, a.RelatedVehicle)).ToList(),
            OperationItems: s.OperationItems.Select(o =>
                new DashboardOperationItemResource(o.Id, o.Label, o.Value, o.Helper, o.Progress, o.Icon)).ToList(),
            Activities: s.Activities.Select(a =>
                new DashboardActivityResource(a.Id, a.Time, a.Title, a.Description, a.Status)).ToList());
    }
}