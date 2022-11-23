using API.ApiModels;
using Application.DTO;
using Application.Responses;
using Application.Services.Interfaces;
using Ardalis.Result.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class TransactionsController : ControllerBase
{
    private readonly ITransactionService _transactionService;
    private readonly IMapper _mapper;

    public TransactionsController(ITransactionService transactionService, IMapper mapper)
    {
        _transactionService = transactionService;
        _mapper = mapper;
    }

    [HttpPost]
    [Route("deposit")]
    public async Task<ActionResult<TransactionResponse>> Deposit([FromBody] DepositRequest depositRequest)
    {
        var transactionDto = _mapper.Map<TransactionDto>(depositRequest);

        var result = await _transactionService.MakeTransaction(transactionDto);
        return this.ToActionResult(result);
    }

    [HttpPost]
    [Route("withdraw")]
    public async Task<ActionResult<TransactionResponse>> Withdraw([FromBody] WithdrawalRequest withdrawRequest)
    {
        var transactionDto = _mapper.Map<TransactionDto>(withdrawRequest);

        var result = await _transactionService.MakeTransaction(transactionDto);
        return this.ToActionResult(result);
    }
}