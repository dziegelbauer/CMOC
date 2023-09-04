namespace CMOC.Domain;

public class Issue
{
    public int Id { get; set; }
    public string TicketNumber { get; set; }
    public string Notes { get; set; }
    public DateTime ExpectedCompletion { get; set; }
}