using CMOC.Domain;
using CMOC.Services.Dto;

namespace CMOC.Services.Utility;

public static class IssueExtensions
{
    public static IssueDto ToDto(this Issue issue)
    {
        var dto = new IssueDto
        {
            Id = issue.Id,
            ExpectedCompletion = issue.ExpectedCompletion,
            Notes = issue.Notes,
            TicketNumber = issue.TicketNumber
        };

        return dto;
    }

    public static Issue ToEntity(this IssueDto dto)
    {
        var issue = new Issue
        {
            Id = dto.Id,
            ExpectedCompletion = dto.ExpectedCompletion,
            Notes = dto.Notes,
            TicketNumber = dto.TicketNumber
        };

        return issue;
    }
}