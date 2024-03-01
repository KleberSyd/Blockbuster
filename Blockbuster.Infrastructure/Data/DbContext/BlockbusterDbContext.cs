using Blockbuster.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Blockbuster.Infrastructure.Data.DbContext;

public class BlockbusterDbContext(DbContextOptions<BlockbusterDbContext> options) : Microsoft.EntityFrameworkCore.DbContext(options)
{
    public required DbSet<Comment> Comments { get; set; }
    public required DbSet<Movie> Movies { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Text)
                .IsRequired();

            entity.HasOne(e => e.Movie)
                .WithMany(m => m.Comments)
                .HasForeignKey(e => e.MovieId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Movie>(entity =>
        {
            entity.HasKey(e => e.Id);

            entity.Property(e => e.Title)
                .IsRequired();

            entity.Property(e => e.Genre)
                .IsRequired();

            entity.Property(e => e.Director)
                .IsRequired();

            entity.Property(e => e.Year)
                .IsRequired();

            entity.Property(e => e.Rating)
                .IsRequired();

            entity.Property(e => e.ImageUrl)
                .IsRequired();

            entity.Property(e => e.TrailerUrl)
                .IsRequired();

            entity.Property(e => e.Description)
                .IsRequired();

            entity.Property(e => e.IsAvailable)
                .IsRequired();

            entity.Property(e => e.Timespan)
                .IsRequired();

            entity.Property(e => e.CreateDateTime)
                .IsRequired();

            entity.Property(e => e.UpdateDateTime)
                .IsRequired(false);
        });
    }
}
