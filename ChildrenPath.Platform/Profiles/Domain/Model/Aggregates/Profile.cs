using ChildrenPath.Platform.Profiles.Domain.Model.Entities;
using ChildrenPath.Platform.Profiles.Domain.Model.Events;
using ChildrenPath.Platform.Profiles.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Model.Aggregates;

namespace ChildrenPath.Platform.Profiles.Domain.Model.Aggregates;

public enum UserRole { Parent, Driver, CompanyAdmin, ChildrenPathAdmin }

public partial class Profile : AggregateRoot
{
    public ProfileId Id { get; private set; }
    public int UserId { get; private set; }

    public string FirstName { get; private set; }
    public string LastName { get; private set; }
    public string Phone { get; private set; }
    public string PhotoUrl { get; private set; }
    public UserRole Role { get; private set; }

    // Composición
    public ParentProfile? ParentProfile { get; private set; }
    public DriverProfile? DriverProfile { get; private set; }
    public CompanyProfile? CompanyProfile { get; private set; }

    public Profile()
    {
        FirstName = string.Empty;
        LastName  = string.Empty;
        Phone     = string.Empty;
        PhotoUrl  = string.Empty;
    }

    public Profile(int userId, string firstName, string lastName, string phone, UserRole role) : this()
    {
        UserId    = userId;
        FirstName = firstName;
        LastName  = lastName;
        Phone     = phone;
        Role      = role;

        // Usamos UserId (disponible pre-save) para evitar el Id = 0.
        AddDomainEvent(new ProfileCreatedEvent(UserId, FirstName, LastName, Role));
    }

    public void UpdateProfile(string firstName, string lastName, string phone)
    {
        FirstName = firstName;
        LastName  = lastName;
        Phone     = phone;
    }

    public void SetParentProfile(string relationship) => ParentProfile = new ParentProfile(relationship);
    public void SetDriverProfile(string license, DateTime expiry) => DriverProfile = new DriverProfile(license, expiry);
    public void SetCompanyProfile(string taxId, string legalName) => CompanyProfile = new CompanyProfile(taxId, legalName);
}