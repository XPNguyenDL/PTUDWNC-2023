using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings;

public class SubscriberMap : IEntityTypeConfiguration<Subscriber>
{
    
    public void Configure(EntityTypeBuilder<Subscriber> builder)
    {
        builder.ToTable("Subscribers");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Email)
            .IsRequired();

        builder.Property(p => p.Reason)
            .HasMaxLength(512);

        builder.Property(p => p.Note)
            .HasMaxLength(512);

        builder.Property(a => a.DateSubscribe)
            .HasColumnType("datetime");

        builder.Property(a => a.DateUnSubscribe)
            .HasColumnType("datetime");

    }
}