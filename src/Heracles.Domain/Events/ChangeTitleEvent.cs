using Heracles.Domain.Common;

namespace Heracles.Domain.Events;

public class ChangeTitleEvent(int todoId, string newTitle) : IDomainEvent
{
    public int TodoId { get; } = todoId;
    public string NewTitle { get; } = newTitle;
    public DateTime OccuredOn { get; } = DateTime.UtcNow;
}