namespace ChildrenPath.Platform.Dashboard.Domain;

// --- ENUMS ---
public enum DashboardType { Parent, Driver, Company, Administrator }
public enum WidgetType { MapWidget, MetricWidget, AlertWidget, ChartWidget, TableWidget }

// --- ENTIDADES ---

public class Widget
{
    public int Id { get; set; }
    public WidgetType Type { get; set; }
    public string Config { get; set; } = string.Empty; // JSON string con la configuración
    
    public string Render() => $"Rendering widget: {Type}";
}

public class KPI
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Value { get; set; }
    public string Unit { get; set; } = string.Empty;
    
    public decimal Calculate() => Value; 
}

public class DashboardSummary
{
    public int Id { get; set; }
    public int ActiveTrips { get; set; }
    public int ActiveVehicles { get; set; }
    public int PendingAlerts { get; set; }

    public void Generate() { /* Lógica para recalcular datos */ }
}

// --- ENTIDAD PRINCIPAL (Aggregate Root) ---
public class Dashboard
{
    public int Id { get; set; }
    public DashboardType Type { get; set; }
    public DateTime LastUpdated { get; set; } = DateTime.UtcNow;

    // Composición (según tu diagrama)
    public List<Widget> Widgets { get; set; } = new();
    public List<KPI> Kpis { get; set; } = new();
    public DashboardSummary Summary { get; set; } = new();

    public void Load() { /* Lógica de carga */ }
    public void Refresh() => LastUpdated = DateTime.UtcNow;
}