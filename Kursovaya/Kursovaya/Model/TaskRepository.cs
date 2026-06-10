namespace Kursovaya.Model;
using Newtonsoft.Json;

public class TaskRepository
{
    private List<Task> _tasks = new List<Task>();
    private readonly object _sync = new();
    public void AddTask()
    {
        lock (_sync)
        {
            _tasks.Add(new Task());
        }
    }
    public bool UpdateTask(Task taskup)
    {
        lock (_sync)
        {
            var ind=_tasks.FindIndex(x => x.Id == taskup.Id);
            if (ind < 0) return false;
            _tasks[ind]=taskup;
            return true;
        }
    }
    public void RemoveTask(Task task)
    {
        lock (_sync)
        {
            _tasks.Remove(task);
        }
    }
    public List<Task> GetTasks()
    {
        lock (_sync)
        {
            return _tasks;
        }
    }
}