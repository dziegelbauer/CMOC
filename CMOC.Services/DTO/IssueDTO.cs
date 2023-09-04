namespace CMOC.Services.Dto;

public class IssueDto
{
    public int Id { get; set; }
    public string TicketNumber { get; set; }
    public string Notes { get; set; }
    public DateTime ExpectedCompletion { get; set; }
}