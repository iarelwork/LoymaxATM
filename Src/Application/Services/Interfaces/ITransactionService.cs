using Application.DTO;
using Application.Responses;
using Ardalis.Result;

namespace Application.Services.Interfaces;

public interface ITransactionService
{
    public Task<Result<TransactionResponse>> MakeTransaction(TransactionDto withdrawalDto);
}