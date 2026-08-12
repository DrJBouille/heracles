using Heracles.Domain.Common;

namespace Heracles.Domain.Events;

public class ToggleCompleteEvent(int todoId, bool isCompleted) : IDomainEvent
{
    public int TodoId { get; } = todoId;
    public bool IsCompleted { get; } = isCompleted;
    public DateTime OccuredOn { get; } =  DateTime.UtcNow;
}