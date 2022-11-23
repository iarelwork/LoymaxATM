using System.ComponentModel.DataAnnotations;

namespace API.ApiModels;

public record CreateAccountRequest()
{
    [Required]
    [StringLength(100)]
    public string FirstName { get; set; }
    [Required]
    [StringLength(100)]
    public string LastName { get; set; } 

    [Required]
    [StringLength(100)]
    public string Patronymic { get; set; }

    [Required]
    public DateTime DateOfBirth { get; set; }
}