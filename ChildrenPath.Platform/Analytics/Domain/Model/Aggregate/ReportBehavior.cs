using ChildrenPath.Platform.Analytics.Domain.Model.Entities;

namespace ChildrenPath.Platform.Analytics.Domain.Model.Aggregate;

public partial class Report
{
    // Composición (según el UML original)
    public ICollection<Metric> Metrics { get; } = new List<Metric>();
    public ICollection<KPI> Kpis { get; } = new List<KPI>();

    public void Generate()
    {
        GeneratedAt = DateTime.UtcNow;
        // Aquí podrías agregar la lógica real de cálculo si fuera necesario
    }

    public string Export(string format)
    {
        return $"Exported report {Id} to {format.ToUpper()}";
    }

    // Métodos para agregar elementos a la composición
    public void AddMetric(Metric metric) => Metrics.Add(metric);
    public void AddKpi(KPI kpi) => Kpis.Add(kpi);
}