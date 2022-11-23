using API.ApiModels;
using Application.DTO;
using Application.Responses;
using Application.Services.Interfaces;
using Ardalis.Result;
using Ardalis.Result.AspNetCore;
using MapsterMapper;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers;

[ApiController]
[Route("[controller]")]
public class AccountsController : ControllerBase
{
    private readonly IAccountService _accountService;
    private readonly IMapper _mapper;

    public AccountsController(IAccountService accountService, IMapper mapper)
    {
        _accountService = accountService;
        _mapper = mapper;
    }

    [HttpPost]
    public async Task<ActionResult<int>> Create([FromBody]CreateAccountRequest accountRequest)
    {
        var createAccountDto = _mapper.Map<CreateAccountDto>(accountRequest);
        var result = await _accountService.Create(createAccountDto);
        return this.ToActionResult(result);
    }

    [HttpGet]
    [Route("{accountId:int}")]
    public async Task<ActionResult<GetAccountResponse>> GetById(int accountId)
    {
        var result = await _accountService.GetById(accountId);
        return this.ToActionResult(result);
    }

    [HttpGet]
    [Route("{accountId:int}/balance")]
    public async Task<ActionResult<decimal>> GetBalance(int accountId)
    {
        var result = await _accountService.GetBalance(accountId);
        return this.ToActionResult(result);
    }
}