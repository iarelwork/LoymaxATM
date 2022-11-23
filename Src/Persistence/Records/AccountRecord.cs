using System.ComponentModel.DataAnnotations;

namespace Persistence.Records;

public class AccountRecord
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Patronymic { get; set; }
    public DateTime DateOfBirth { get; set; }
    public decimal Balance { get; set; }
    public virtual List<TransactionRecord> Transactions { get; set; } = new();

    [ConcurrencyCheck]
    public Guid Version { get; set; }

}