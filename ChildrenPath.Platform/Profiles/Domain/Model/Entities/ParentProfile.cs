namespace ChildrenPath.Platform.Profiles.Domain.Model.Entities;

public class ParentProfile
{
    public int Id { get; private set; }
    public string Relationship { get; private set; }
    public int ChildrenCount { get; private set; }

    public ParentProfile() { Relationship = string.Empty; }

    public ParentProfile(string relationship)
    {
        Relationship = relationship;
        ChildrenCount = 0;
    }
}