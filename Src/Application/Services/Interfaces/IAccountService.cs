using Application.DTO;
using Application.Responses;
using Ardalis.Result;

namespace Application.Services.Interfaces;

public interface IAccountService
{
    public Task<Result<GetAccountResponse>> GetById(int id);


    public Task<Result<int>> Create(CreateAccountDto createAccountDto);


    public Task<Result<decimal>> GetBalance(int accountId);

}