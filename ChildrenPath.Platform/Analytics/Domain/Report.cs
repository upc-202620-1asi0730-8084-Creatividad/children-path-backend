namespace ChildrenPath.Platform.Analytics.Domain;

// --- ENUM ---
public enum ReportType { Attendance, Punctuality, Performance, Journey, Incidents }

// --- VALUE OBJECTS (Entidades compuestas) ---
public class Metric
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    public int ReportId { get; set; } // FK

    public decimal Calculate() => Value;
}

public class KPI
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Target { get; set; }
    public decimal Actual { get; set; }
    public int ReportId { get; set; } // FK

    public bool Evaluate() => Actual >= Target;
}

// --- ENTIDAD PRINCIPAL (Aggregate Root) ---
public class Report
{
    public int Id { get; set; }
    public ReportType Type { get; set; }
    public DateTime GeneratedAt { get; set; } = DateTime.UtcNow;
    public string Period { get; set; } = string.Empty;

    // Composición (según tu UML)
    public List<Metric> Metrics { get; set; } = new();
    public List<KPI> Kpis { get; set; } = new();

    // Métodos del UML
    public void Generate()
    {
        GeneratedAt = DateTime.UtcNow;
        // Lógica de negocio para calcular métricas reales
    }

    public string Export(string format)
    {
        // Simula la exportación a PDF o CSV
        return $"Exported report {Id} to {format}";
    }
}