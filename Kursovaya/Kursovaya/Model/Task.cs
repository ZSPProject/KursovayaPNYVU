namespace Kursovaya.Model;

public class Task
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public string Title { get; set; } = "";

    public string Description { get; set; } = "";

    public DateTime DueDate { get; set; } = DateTime.Now;

    public bool IsCompleted { get; set; }
}