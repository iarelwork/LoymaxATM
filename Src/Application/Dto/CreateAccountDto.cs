namespace Application.DTO;

public record CreateAccountDto
{
    public string FirstName { get; set; }

    public string LastName { get; set; }

    public string Patronymic { get; set; }

    public DateTime DateOfBirth { get; set; }
}