using Application.Enums;

namespace Application.Responses;

public record TransactionResponse
{
    public int Id { get; set; }

    public string Status { get; set; }
}