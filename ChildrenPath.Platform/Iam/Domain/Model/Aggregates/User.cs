using System.Text.Json.Serialization;
using ChildrenPath.Platform.Iam.Domain.Model.Events;
using ChildrenPath.Platform.Iam.Domain.Model.ValueObjects;
using ChildrenPath.Platform.Shared.Domain.Model.Aggregates;

namespace ChildrenPath.Platform.Iam.Domain.Model.Aggregates;

public partial class User : AggregateRoot
{
    // Constructor vacío requerido por EF Core
    public User() : this(string.Empty, string.Empty)
    {
    }

    public User(string username, string passwordHash)
    {
        Username = username;
        PasswordHash = passwordHash;

        // Si el nombre de usuario no está vacío, emitimos el evento
        if (!string.IsNullOrWhiteSpace(username))
        {
            AddDomainEvent(new UserRegisteredEvent(Id, username));
        }
    }

    public UserId Id { get; }
    public string Username { get; private set; }

    [JsonIgnore] 
    public string PasswordHash { get; private set; }

    // --- Comportamiento del Dominio ---
    
    public User UpdateUsername(string username)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(username);
        Username = username;
        return this;
    }

    public User UpdatePasswordHash(string passwordHash)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(passwordHash);
        PasswordHash = passwordHash;
        return this;
    }
}