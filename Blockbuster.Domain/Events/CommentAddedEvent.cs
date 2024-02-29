using Blockbuster.Domain.Models;

namespace Blockbuster.Domain.Events;

public class CommentAddedEvent
{
    public required Comment Comment { get; set; }
}