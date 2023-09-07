namespace CMOC.Services.Dto;

public class IssueDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public string Notes { get; set; } = string.Empty;
    public DateTime ExpectedCompletion { get; set; }
}