using System.ComponentModel.DataAnnotations;

namespace API.ApiModels;

public record WithdrawalRequest
{
    [Required]
    [Range(1, int.MaxValue)]
    public int AccountId { get; set; }

    [Required]
    [Range(0, 9999999999999999.99)]
    public decimal WithdrawalAmount { get; set; }
}