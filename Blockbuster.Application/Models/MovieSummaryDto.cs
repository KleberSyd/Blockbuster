namespace Blockbuster.Application.Models;

public record MovieSummaryDto
{
    public int Id { get; set; }
    public string? ImageUrl { get; set; }
    public required string Title { get; set; }
}