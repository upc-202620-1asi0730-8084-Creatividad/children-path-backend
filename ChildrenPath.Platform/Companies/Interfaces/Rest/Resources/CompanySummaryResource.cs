namespace ChildrenPath.Platform.Companies.Interfaces.Rest.Resources;

public record CompanySummaryResource(
    int ActiveCompanies,
    int TotalVehicles,
    int ActiveDrivers,
    int LinkedSchools,
    int ManagedStudents,
    int ComplianceScore,
    int ServiceQualityScore,
    int PendingReviews
);
