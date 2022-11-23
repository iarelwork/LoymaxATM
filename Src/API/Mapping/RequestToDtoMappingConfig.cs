using API.ApiModels;
using Application.DTO;
using Application.Enums;
using Mapster;

namespace API.Mapping
{
    public class RequestToDtoMappingConfig : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<WithdrawalRequest, TransactionDto>()
                .Map(dest => dest.AccountId, src => src.AccountId)
                .Map(dest => dest.Amount, src => src.WithdrawalAmount)
                .AfterMapping(dest => dest.Type = TransactionTypeDto.Withdrawal);

            config.NewConfig<DepositRequest, TransactionDto>()
                .Map(dest => dest.AccountId, src => src.AccountId)
                .Map(dest => dest.Amount, src => src.DepositAmount)
                .AfterMapping(dest => dest.Type = TransactionTypeDto.Deposit);
        }
    }
}
