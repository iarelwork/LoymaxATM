using Application.DTO;
using Application.Enums;
using Application.Responses;
using Application.Services.Interfaces;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Records;

namespace Application.Services;

public class TransactionService : ITransactionService
{
    private readonly AppDbContext _appContext;

    public TransactionService(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<Result<TransactionResponse>> MakeTransaction(TransactionDto transactionDto)
    {
        var account = await _appContext.Accounts.FindAsync(transactionDto.AccountId);

        if(account == null)
            return Result<TransactionResponse>.Error($"No account with such id {transactionDto.AccountId}");

        if(transactionDto.Type == TransactionTypeDto.Withdrawal && account.Balance < transactionDto.Amount)
            return Result<TransactionResponse>.Error($"Not enough money for transaction");

        var transaction =
            (await _appContext.Transactions.AddAsync(new TransactionRecord
            {
                AccountId = transactionDto.AccountId,
                Amount = transactionDto.Amount,
                Type = (int)transactionDto.Type,
                Status = (int)TransactionStatusDto.Processing,
            })).Entity;
        await _appContext.SaveChangesAsync();

        if (!await TryChangeBalance(account, transactionDto))
        {
            _appContext.Transactions.First(t => t.Id == transaction.Id).Status = (int)TransactionStatusDto.Failed;
            await _appContext.SaveChangesAsync();
            return Result<TransactionResponse>.Error("Transaction was failed due concurrency error. Try later");
        }

        transaction.Status = (int)TransactionStatusDto.Success;
        await _appContext.SaveChangesAsync();
        return new Result<TransactionResponse>(
            new TransactionResponse
            {
                Id = transaction.Id,
                Status = TransactionStatusDto.Success.ToString(),
            });
    }

    private async Task<bool> TryChangeBalance(AccountRecord account, TransactionDto transactionDto, int numberOfRetries = 100)
    {
        bool isSaveSuccess = true;

        if (transactionDto.Type == TransactionTypeDto.Deposit)
            account.Balance += transactionDto.Amount;

        if (transactionDto.Type == TransactionTypeDto.Withdrawal)
            account.Balance -= transactionDto.Amount;

        // "optimistic concurrency" with concurrent token
        account.Version = Guid.NewGuid();
        for (int i = 0; i < numberOfRetries; i++)
        {
            try
            {
                await _appContext.SaveChangesAsync();
                break;
            }
            // ignore all conflicting entities
            catch (DbUpdateConcurrencyException ex)
            {
                isSaveSuccess = false;
                ex.Entries.ToList().ForEach(e => e.State = EntityState.Detached);
            }
        }

        return isSaveSuccess;
    }
}