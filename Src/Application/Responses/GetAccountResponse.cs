namespace Application.Responses;

public record GetAccountResponse(int Id, string FirstName, string LastName, string Patronymic, DateTime DateOfBirth)
{
    public int Id { get; set; } = Id;

    public string FirstName { get; set; } = FirstName;

    public string LastName { get; set; } = LastName;

    public string Patronymic { get; set; } = Patronymic;

    public DateTime DateOfBirth { get; set; } = DateOfBirth;
}