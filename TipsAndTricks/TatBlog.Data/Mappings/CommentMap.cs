using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings;

public class CommentMap : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("Comments");

        builder.HasKey(x => x.Id);
        builder.Property(s => s.Active)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(s => s.Content)
            .HasMaxLength(1024)
            .IsRequired();

        builder.Property(s => s.PostTime)
            .HasColumnType("datetime");

        builder.Property(s => s.UserComment)
            .HasMaxLength(512);


    }
}