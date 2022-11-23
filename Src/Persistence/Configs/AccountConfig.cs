using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Records;

namespace Persistence.Configs;

public class AccountConfig : IEntityTypeConfiguration<AccountRecord>
{
    public void Configure(EntityTypeBuilder<AccountRecord> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.FirstName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.LastName)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.Patronymic)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(p => p.DateOfBirth)
            .IsRequired();

        builder.Property(p => p.Balance);

        builder.HasKey("Id");

        builder
            .HasMany(acc => acc.Transactions)
            .WithOne(transaction => transaction.Account)
            .HasForeignKey(transaction => transaction.AccountId);

        builder.ToTable("Accounts");
    }
}