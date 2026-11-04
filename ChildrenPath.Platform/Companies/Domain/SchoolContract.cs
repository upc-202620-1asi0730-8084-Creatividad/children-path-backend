public class SchoolContract
{
    public int Id { get; set; }
    public int CompanyId { get; set; }
    public string SchoolName { get; set; } = string.Empty;
    public string District { get; set; } = string.Empty;
    public int RouteCount { get; set; }
    public int StudentCount { get; set; }
    public string ContactName { get; set; } = string.Empty;
    public DateTime RenewalDate { get; set; }
    public string Status { get; set; } = "active"; // active, renewal, review, paused
    public int Score { get; set; }
    public string Notes { get; set; } = string.Empty;
}