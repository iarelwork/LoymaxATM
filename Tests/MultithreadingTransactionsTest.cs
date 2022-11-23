using Application.DTO;
using Application.Enums;
using Application.Services;
using Microsoft.EntityFrameworkCore;
using Persistence;
using Xunit;

namespace Tests;

public class MultithreadingTransactionsTest
{
    private const int ACCOUNTS_COUNT = 50;
    private const int THREADS_COUNT = 10;
    
    private static readonly DbContextOptions<AppDbContext> DbContextOptions =
        new DbContextOptionsBuilder<AppDbContext>().UseInMemoryDatabase("TestInMemoryDb").Options;

    [Fact]
    public async Task TaskTest()
    {
        // Arrange
        var accountService = CreateAccountService();
        var accounts = await GenerateAccounts();
        var tasks = GenerateTasks(accounts);

        // Act
        tasks.ForEach(task => task.Start());
        await Task.WhenAll(tasks);

        List<decimal> transactionsSums = new();
        List<decimal> balancesSums = new();

        var context = CreateDbContext();
        foreach (int id in accounts)
        {
            var depositsSumsByTransactions = context.Transactions
                .Where(t => t.AccountId == id)
                .Where(t => (TransactionStatusDto)t.Status == TransactionStatusDto.Success)
                .Where(t => (TransactionTypeDto)t.Type == TransactionTypeDto.Deposit)
                .Select(t => t.Amount)
                .Sum();
            var withdrawalsSumsByTransactions = context.Transactions
                .Where(t => t.AccountId == id)
                .Where(t => (TransactionStatusDto)t.Status == TransactionStatusDto.Success)
                .Where(t => (TransactionTypeDto)t.Type == TransactionTypeDto.Withdrawal)
                .Select(t => t.Amount)
                .Sum();
            transactionsSums.Add(depositsSumsByTransactions - withdrawalsSumsByTransactions);

            balancesSums.Add(await accountService.GetBalance(id));
        }

        var countOfTransactionsInUnExpectedState =
            context.Transactions.Count(t => t.Status == (int)TransactionStatusDto.Processing);

        // Assert
        Assert.True(transactionsSums.SequenceEqual(balancesSums));
        Assert.Equal(0, countOfTransactionsInUnExpectedState);
    }

    private async Task<List<int>> GenerateAccounts()
    {
        var accountService = CreateAccountService();
        var result = new List<int>();

        for (int i = 0; i < ACCOUNTS_COUNT; i++)
        {
            var idResult = await accountService.Create(new CreateAccountDto
            {
                FirstName = "test",
                LastName = "test",
                Patronymic = "test",
                DateOfBirth = DateTime.Now.AddYears(-50)
            });
            result.Add(idResult);
        }
        return result;
    }

    private List<Task> GenerateTasks(List<int> accountsIds)
    {
        var result = new List<Task>();

        foreach(int id in accountsIds)
        {
            for (int j = 0; j < THREADS_COUNT/2; j++)
            {
                var transactionService1 = CreateTransactionService();
                var transactionService2 = CreateTransactionService();
                var deposit = new TransactionDto { AccountId = id, Amount = 50, Type = TransactionTypeDto.Deposit };
                var withdrawal = new TransactionDto { AccountId = id, Amount = 50, Type = TransactionTypeDto.Withdrawal };
                result.Add(new Task(async () => await transactionService1.MakeTransaction(deposit)));
                result.Add(new Task(async () => await transactionService2.MakeTransaction(withdrawal)));
            }
        }

        return result;
    }

    private AccountService CreateAccountService()
    {
        return new AccountService(CreateDbContext());
    }

    private TransactionService CreateTransactionService()
    {
        return new TransactionService(CreateDbContext());
    }

    private AppDbContext CreateDbContext()
    {
        return new AppDbContext(DbContextOptions);
    }
}