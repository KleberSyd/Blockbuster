using System.Diagnostics.CodeAnalysis;

namespace Blockbuster.Domain.Entities;

public class Movie
{
    // EF Core requires a parameterless constructor
    protected Movie() { }

    [SetsRequiredMembers]
    public Movie(string title, string genre, string director, int year, int rating, string imageUrl,
        string trailerUrl, string description, bool isAvailable)
    {
        Title = title;
        Genre = genre;
        Director = director;
        Year = year;
        Rating = rating;
        ImageUrl = imageUrl;
        TrailerUrl = trailerUrl;
        Description = description;
        IsAvailable = isAvailable;
    }
    public int Id{ get; set; }
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
    public DateTime CreateDateTime{ get; set; }
    public DateTime? UpdateDateTime { get; set; }
    public IEnumerable<Comment>? Comments { get; set; }
}