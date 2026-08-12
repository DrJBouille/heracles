using Heracles.Domain.Common;
using Heracles.Domain.Events;

namespace Heracles.Domain.Entities;

public class Todo : Entity
{
    public string Title { get; private set; }
    public bool IsCompleted { get; private set; }
    public int OwnerId { get; private set; }
    public DateTime CreatedAt  { get; private set; }
    
    private Todo() { }
    
    public static Todo Create(string title, int ownerId)
    {
        if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Title must be specified");
        
        return new Todo
        {
            Title = title,
            IsCompleted = false,
            OwnerId = ownerId,
            CreatedAt = DateTime.UtcNow 
        };
    }
    
    public void ToggleCompleted() 
    {
        IsCompleted = !IsCompleted;
        AddDomainEvent(new ToggleCompleteEvent(Id, IsCompleted));
    }
    
    public void Rename(string newTitle)
    {
        if (string.IsNullOrWhiteSpace(newTitle)) throw new ArgumentException("Title must be specified");
        Title = newTitle;
        AddDomainEvent(new ChangeTitleEvent(Id, newTitle));
    }
}