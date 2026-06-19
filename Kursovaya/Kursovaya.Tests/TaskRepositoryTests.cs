using Kursovaya.Model;
using TaskModel = Kursovaya.Model.Task;
namespace Kursovaya.Tests;

public class TaskRepositoryTests
{
    [Fact]
    public void AddTask_Should_Add_Task()
    {
        var repo = new TaskRepository();

        var task = new TaskModel
        {
            Title = "Test Task",
            Description = "Test Description",
            DueDate = DateTime.Now
        };

        repo.AddTask(task);

        Assert.Contains(task, repo.GetTasks());
    }

    [Fact]
    public void RemoveTask_Should_Remove_Task()
    {
        var repo = new TaskRepository();

        var task = new TaskModel
        {
            Title = "Test Task"
        };

        repo.AddTask(task);

        bool result = repo.RemoveTask(task);

        Assert.True(result);
        Assert.DoesNotContain(task, repo.GetTasks());
    }

    [Fact]
    public void UpdateTask_Should_Update_Task()
    {
        var repo = new TaskRepository();

        var task = new TaskModel
        {
            Title = "Old Title"
        };

        repo.AddTask(task);

        task.Title = "New Title";

        bool result = repo.UpdateTask(task);

        Assert.True(result);

        var updatedTask = repo.GetTasks()
                              .First(x => x.Id == task.Id);

        Assert.Equal("New Title", updatedTask.Title);
    }

    [Fact]
    public void GetTasks_Should_Return_Added_Tasks()
    {
        var repo = new TaskRepository();

        repo.AddTask(new TaskModel { Title = "Task 1" });
        repo.AddTask(new TaskModel { Title = "Task 2" });

        var tasks = repo.GetTasks();

        Assert.True(tasks.Count >= 2);
    }

    [Fact]
    public void RemoveTask_Should_Return_False_When_Task_Not_Found()
    {
        var repo = new TaskRepository();

        var task = new TaskModel
        {
            Title = "Ghost Task"
        };

        bool result = repo.RemoveTask(task);

        Assert.False(result);
    }
}