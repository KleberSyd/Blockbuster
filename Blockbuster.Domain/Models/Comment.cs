using System.Diagnostics.CodeAnalysis;

namespace Blockbuster.Domain.Models;

public class Comment
{
    protected Comment() { }

    [SetsRequiredMembers]
    public Comment(string text, int movieId, string userId, DateTime dateTime)
    {
        Text = text;
        MovieId = movieId;
        UserId = userId;
        DateTime = dateTime;
    }

    public int Id { get; set; }
    public string Text { get; set; }
    public int MovieId { get; set; }
    public Movie Movie { get; set; }
    public string UserId { get; set; }
    public DateTime DateTime { get; set; }
}