using Application.Enums;

namespace Application.DTO;

public record TransactionDto
{
    public int AccountId { get; set; }

    public TransactionTypeDto Type { get; set; }

    public decimal Amount { get; set; }
}