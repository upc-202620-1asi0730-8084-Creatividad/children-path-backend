namespace ChildrenPath.Platform.Analytics.Domain.Model.Entities;

/// <summary>
///     Represents a Key Performance Indicator evaluated in a report.
/// </summary>
public class KPI
{
    public KPI() { }

    public KPI(string code, string description, decimal target, decimal actual)
    {
        Code = code;
        Description = description;
        Target = target;
        Actual = actual;
    }

    public int Id { get; set; }
    public int ReportId { get; set; } // FK
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Target { get; set; }
    public decimal Actual { get; set; }

    public bool Evaluate() => Actual >= Target;
}