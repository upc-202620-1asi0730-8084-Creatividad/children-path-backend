using ChildrenPath.Platform.Analytics.Domain.Model.Commands;
using ChildrenPath.Platform.Analytics.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;

/// <summary>
///     Represents the Report aggregate root within the Analytics Bounded Context.
/// </summary>
public partial class Report
{
    public Report()
    {
        Period = string.Empty;
        GeneratedAt = DateTime.UtcNow;
    }

    public Report(ReportType type, string period) : this()
    {
        Type = type;
        Period = period;
    }

    public Report(CreateReportCommand command) : this(command.Type, command.Period)
    {
    }

    public int Id { get; set; }
    public ReportType Type { get; private set; }
    public string Period { get; private set; }
    public DateTime GeneratedAt { get; private set; }
}