namespace ChildrenPath.Platform.Companies.Domain.Model.Entities;

public record CompanySummary(
    int ActiveCompanies,
    int TotalVehicles,
    int ActiveDrivers,
    int LinkedSchools,
    int ManagedStudents,
    int ComplianceScore,
    int ServiceQualityScore,
    int PendingReviews
);
