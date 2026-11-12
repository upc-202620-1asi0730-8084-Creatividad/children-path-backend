namespace ChildrenPath.Platform.Analytics.Domain.Model.Entities;

/// <summary>
///     Represents a specific metric recorded within a report.
/// </summary>
public class Metric
{
    public Metric() { }

    public Metric(string name, decimal value, string unit)
    {
        Name = name;
        Value = value;
        Unit = unit;
    }

    public int Id { get; set; }
    public int ReportId { get; set; } // FK
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;

    public decimal Calculate() => Value;
}