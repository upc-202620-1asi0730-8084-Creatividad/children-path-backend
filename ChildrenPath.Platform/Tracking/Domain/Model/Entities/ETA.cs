namespace ChildrenPath.Platform.Tracking.Domain.Model.Entities;

public class ETA
{
    public int Id { get; private set; }
    public DateTime EstimatedArrival { get; private set; }
    public int MinutesRemaining { get; private set; }
    public int StopId { get; private set; }

    // Constructor vacío requerido por Entity Framework
    public ETA() { }

    // Constructor que pide tu Agregado
    public ETA(DateTime estimatedArrival, int minutesRemaining, int stopId)
    {
        EstimatedArrival = estimatedArrival;
        MinutesRemaining = minutesRemaining;
        StopId = stopId;
    }
}