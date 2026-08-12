namespace Heracles.Domain.Common;

public interface IDomainEvent
{
    DateTime OccuredOn { get; }
}