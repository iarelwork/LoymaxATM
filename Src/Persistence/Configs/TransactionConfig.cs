using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Persistence.Records;

namespace Persistence.Configs;

public class TransactionConfig
{
    public void Configure(EntityTypeBuilder<TransactionRecord> builder)
    {
        builder.Property(p => p.Id)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.AccountId)
            .IsRequired();

        builder.Property(p => p.DateCreated)
            .ValueGeneratedOnAdd();

        builder.Property(p => p.DateUpdated)
            .ValueGeneratedOnAddOrUpdate();

        builder.Property(p => p.Type)
            .IsRequired();

        builder.Property(p => p.Status)
            .IsRequired();

        builder.Property(p => p.Amount)
            .IsRequired();

        builder.HasKey("Id");
        
        builder.ToTable("Transactions");
    }
}