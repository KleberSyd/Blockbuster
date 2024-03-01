namespace Blockbuster.Application.Models;

public record MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Director { get; set; }
    public int Year { get; set; }
    public int Rating { get; set; }
    public string ImageUrl { get; set; }
    public string TrailerUrl { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }
    public int Timespan { get; set; }
}