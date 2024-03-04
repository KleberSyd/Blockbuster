namespace Blockbuster.Application.Models;

public record MovieDto
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Genre { get; set; }
    public string Director { get; set; }
    public int Year { get; set; }
    private int _rating;
    public int Rating
    {
        get => _rating;
        set
        {
            if (value is < 1 or > 10)
                throw new ArgumentException("Rating must be between 1 and 10.");
            _rating = value;
        }
    }
    public string? ImageUrl { get; set; }
    public string? TrailerUrl { get; set; }
    public string Description { get; set; }
    public bool IsAvailable { get; set; }

    private int _timespan;
    public string TimespanText { get; set; }
    public int Timespan
    {
        get => _timespan;
        set
        {
            if (value < 0 || value > 180)
                throw new ArgumentException("Timespan must be between 0 and 180 minutes.");
            _timespan = value;
        }
    }
}