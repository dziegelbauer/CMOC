namespace CMOC.Domain;

public class Issue
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime ExpectedCompletion { get; set; }
}