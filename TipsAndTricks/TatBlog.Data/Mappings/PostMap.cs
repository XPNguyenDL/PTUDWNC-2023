using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using TatBlog.Core.Entities;

namespace TatBlog.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.ToTable("Posts");
            
            builder.HasKey(x => x.Id);

            builder.Property(p => p.Title)
                .IsRequired()
                .HasMaxLength(500);

            builder.Property(p => p.ShortDescription)
                .HasMaxLength(5000)
                .IsRequired();

            builder.Property(p => p.Description)
                .IsRequired()
                .HasMaxLength(5000);

            builder.Property(p => p.UrlSlug)
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(p => p.Meta)
                .HasMaxLength(1000)
                .IsRequired();

            builder.Property(p => p.ImageUrl)
                .HasMaxLength(1000);

            builder.Property(p => p.ViewCount)
                .IsRequired()
                .HasDefaultValue(0);

            builder.Property(p => p.Published)
                .IsRequired()
                .HasDefaultValue(false);

            builder.Property(p => p.PostedDate)
                .HasColumnType("datetime");

            builder.Property(p => p.ModifiedDate)
                .HasColumnType("datetime");

            builder.HasOne(p => p.Category)
                .WithMany(c => c.Posts)
                .HasForeignKey(p => p.CategoryId)
                .HasConstraintName("FK_Posts_Categories")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(p => p.Author)
                .WithMany(t => t.Posts)
                .HasForeignKey(p => p.AuthorId)
                .HasConstraintName("FK_Posts_Authors")
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasMany(t => t.Tags)
                .WithMany(p => p.Posts)
                .UsingEntity(pt => pt.ToTable("PostTags"));
        }
    }
}
