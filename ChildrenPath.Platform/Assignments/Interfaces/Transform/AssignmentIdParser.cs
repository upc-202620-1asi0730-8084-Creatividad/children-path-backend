namespace ChildrenPath.Platform.Assignments.Interfaces.Transform;

/// <summary>
///     Parses assignment identifiers from different string formats,
///     e.g. "42", "AS-042", "as-042".
/// </summary>
public static class AssignmentIdParser
{
    private const string Prefix = "AS-";

    public static bool TryParse(string? rawId, out int assignmentId)
    {
        assignmentId = 0;
        if (string.IsNullOrWhiteSpace(rawId)) return false;

        var cleaned = rawId.Trim();
        if (cleaned.StartsWith(Prefix, StringComparison.OrdinalIgnoreCase))
            cleaned = cleaned[Prefix.Length..];

        return int.TryParse(cleaned, out assignmentId) && assignmentId > 0;
    }
}