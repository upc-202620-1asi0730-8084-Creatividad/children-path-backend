namespace ChildrenPath.Platform.Profiles.Domain.Model.Entities;

public class CompanyProfile
{
    public int Id { get; private set; }
    public string TaxId { get; private set; }
    public string LegalName { get; private set; }

    public CompanyProfile() { TaxId = string.Empty; LegalName = string.Empty; }

    public CompanyProfile(string taxId, string legalName)
    {
        TaxId = taxId;
        LegalName = legalName;
    }
}