using System.ComponentModel.DataAnnotations;

namespace ChildrenPath.Platform.Students.Interfaces.Resources;

public record UpdateStudentResource(
    [Required, StringLength(100)] string FirstName,
    [Required, StringLength(100)] string LastName,
    [Required, StringLength(50)] string Grade,
    [Required, StringLength(150)] string School,
    [Required, StringLength(100)] string GuardianName,
    [Required, StringLength(50)] string GuardianPhone,
    [EmailAddress, StringLength(150)] string? GuardianEmail,
    [StringLength(50)] string? EmergencyContact,
    [StringLength(150)] string? RouteName,
    [StringLength(250)] string? PickupPoint,
    [StringLength(50)] string? PickupWindow,
    [Required, StringLength(20)] string Status);