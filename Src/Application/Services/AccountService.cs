using Application.DTO;
using Application.Responses;
using Application.Services.Interfaces;
using Ardalis.Result;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Persistence.Records;

namespace Application.Services;

public class AccountService : IAccountService
{
    private readonly AppDbContext _appContext;

    public AccountService(AppDbContext appContext)
    {
        _appContext = appContext;
    }

    public async Task<Result<GetAccountResponse>> GetById(int accountId)
    {
        var accountRecord = await _appContext.Accounts.FindAsync(accountId);
        if (accountRecord == null)
            return Result<GetAccountResponse>.Error($"No account with such id {accountId}");

        return new Result<GetAccountResponse>(
            new GetAccountResponse(
                accountRecord.Id, 
                accountRecord.FirstName, 
                accountRecord.LastName, 
                accountRecord.Patronymic,
                accountRecord.DateOfBirth));
    }

    public async Task<Result<int>> Create(CreateAccountDto createAccountDto)
    {
        var accountRecord = new AccountRecord
        {
            FirstName = createAccountDto.FirstName,
            LastName = createAccountDto.LastName,
            Patronymic = createAccountDto.Patronymic,
            DateOfBirth = createAccountDto.DateOfBirth,
            Version = Guid.NewGuid(),
        };
        await _appContext.Accounts.AddAsync(accountRecord);
        await _appContext.SaveChangesAsync();

        return new Result<int>(accountRecord.Id);
    }

    public async Task<Result<decimal>> GetBalance(int accountId)
    {
        var accountRecord = await _appContext.Accounts.FindAsync(accountId);
        if(accountRecord == null)
            return Result.Error($"No account with such id {accountId}");

        return new Result<decimal>(accountRecord.Balance);
    }
}