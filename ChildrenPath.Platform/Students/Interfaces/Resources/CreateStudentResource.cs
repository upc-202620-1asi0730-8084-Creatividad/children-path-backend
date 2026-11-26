using System.ComponentModel.DataAnnotations;

namespace ChildrenPath.Platform.Students.Interfaces.Resources;

public record CreateStudentResource(
    [Required, StringLength(100)] string FirstName,
    [Required, StringLength(100)] string LastName,
    [Required, StringLength(50)] string Grade,
    [Required, StringLength(150)] string School,
    [Required, StringLength(100)] string GuardianName,
    [Required, StringLength(50)] string GuardianPhone,
    [EmailAddress, StringLength(150)] string? GuardianEmail = null,
    [StringLength(50)] string? EmergencyContact = null,
    [StringLength(150)] string? RouteName = null,
    [StringLength(250)] string? PickupPoint = null,
    [StringLength(50)] string? PickupWindow = null,
    [Required, StringLength(20)] string Status = "unassigned");