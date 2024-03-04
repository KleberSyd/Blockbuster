namespace Blockbuster.Application.Models;

public record MovieEditDto
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public int Rating { get; set; }
    public string? ImageUrl { get; set; }
    public int Timespan { get; set; }
}