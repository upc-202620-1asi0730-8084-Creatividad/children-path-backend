using ChildrenPath.Platform.Assignments.Domain.Model.ValueObjects;

namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Converts between frontend string keys (lowercase / kebab-case) and Assignments domain enums.
/// </summary>
public static class AssignmentEnumMapper
{
    public static AssignmentShift ParseShift(string? raw, bool defaultToMorningWhenEmpty = false)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return defaultToMorningWhenEmpty ? AssignmentShift.Morning : (AssignmentShift)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "morning" => AssignmentShift.Morning,
            "afternoon" => AssignmentShift.Afternoon,
            _ => (AssignmentShift)(-1)
        };
    }

  public static AssignmentStatus ParseStatus(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (AssignmentStatus)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "validated" => AssignmentStatus.Validated,
            "pending" => AssignmentStatus.Pending,
            "conflict" => AssignmentStatus.Conflict,
            "inactive" => AssignmentStatus.Inactive,
            _ => (AssignmentStatus)(-1)
        };
    }

    public static AssignmentValidation ParseValidation(string? raw)
    {
        if (string.IsNullOrWhiteSpace(raw))
            return (AssignmentValidation)(-1);

        return raw.Trim().ToLowerInvariant() switch
        {
            "ready" => AssignmentValidation.Ready,
            "capacity-risk" or "capacityrisk" or "capacity_risk" => AssignmentValidation.CapacityRisk,
            "schedule-conflict" or "scheduleconflict" or "schedule_conflict" => AssignmentValidation.ScheduleConflict,
            "missing-route" or "missingroute" or "missing_route" => AssignmentValidation.MissingRoute,
            "inactive" => AssignmentValidation.Inactive,
            _ => (AssignmentValidation)(-1)
        };
    }

    public static string ToFrontendShift(AssignmentShift shift) => shift switch
    {
        AssignmentShift.Morning => "morning",
        AssignmentShift.Afternoon => "afternoon",
        _ => "morning"
    };

    public static string ToFrontendStatus(AssignmentStatus status) => status switch
    {
        AssignmentStatus.Validated => "validated",
        AssignmentStatus.Pending => "pending",
        AssignmentStatus.Conflict => "conflict",
        AssignmentStatus.Inactive => "inactive",
        _ => "pending"
    };

    public static string ToFrontendValidation(AssignmentValidation validation) => validation switch
    {
        AssignmentValidation.Ready => "ready",
        AssignmentValidation.CapacityRisk => "capacity-risk",
        AssignmentValidation.ScheduleConflict => "schedule-conflict",
        AssignmentValidation.MissingRoute => "missing-route",
        AssignmentValidation.Inactive => "inactive",
        _ => "ready"
    };
}