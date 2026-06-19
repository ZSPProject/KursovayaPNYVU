using Kursovaya.Model;
using Kursovaya.ViewModel;
using TaskModel = Kursovaya.Model.Task;
namespace Kursovaya.Tests;

public class MainViewModelTests
{
    [Fact]
    public void Constructor_Should_Create_Collection()
    {
        var vm = new MainViewModel();

        Assert.NotNull(vm.Tasks);
    }

    [Fact]
    public void SelectedFilter_Default_Should_Be_All()
    {
        var vm = new MainViewModel();

        Assert.Equal("Все", vm.SelectedFilter);
    }

    [Fact]
    public void Title_Should_Be_Assigned()
    {
        var vm = new MainViewModel();

        vm.Title = "Homework";

        Assert.Equal("Homework", vm.Title);
    }

    [Fact]
    public void Description_Should_Be_Assigned()
    {
        var vm = new MainViewModel();

        vm.Description = "Description";

        Assert.Equal("Description", vm.Description);
    }

    [Fact]
    public void AddTaskCommand_Should_Add_Task()
    {
        var vm = new MainViewModel();

        int before = vm.Tasks.Count;

        vm.Title = "Task 1";
        vm.Description = "Description";

        vm.AddTaskCommand.Execute(null);

        Assert.Equal(before + 1, vm.Tasks.Count);
    }

    [Fact]
    public void AddTask_Should_Not_Add_Empty_Title()
    {
        var vm = new MainViewModel();

        int before = vm.Tasks.Count;

        vm.Title = "";

        vm.AddTaskCommand.Execute(null);

        Assert.Equal(before, vm.Tasks.Count);
    }

    [Fact]
    public void DeleteTaskCommand_Should_Remove_Task()
    {
        var vm = new MainViewModel();

        vm.Title = "Task To Delete";

        vm.AddTaskCommand.Execute(null);

        var task = vm.Tasks.Last();

        vm.SelectedTask = task;

        int before = vm.Tasks.Count;

        vm.DeleteTaskCommand.Execute(null);

        Assert.Equal(before - 1, vm.Tasks.Count);
    }

    [Fact]
    public void SortByDate_Should_Sort_Ascending()
    {
        var vm = new MainViewModel();

        vm.Tasks.Clear();

        vm.Tasks.Add(new TaskModel
        {
            Title = "Later",
            DueDate = new DateTime(2026, 12, 31)
        });

        vm.Tasks.Add(new TaskModel
        {
            Title = "Earlier",
            DueDate = new DateTime(2026, 1, 1)
        });

        vm.SortByDateCommand.Execute(null);

        Assert.Equal("Earlier", vm.Tasks.First().Title);
    }

    [Fact]
    public void SelectedTask_Should_Copy_Data_To_Editor()
    {
        var vm = new MainViewModel();

        var task = new TaskModel
        {
            Title = "Selected",
            Description = "Description",
            DueDate = new DateTime(2026, 5, 1),
            IsCompleted = true
        };

        vm.SelectedTask = task;

        Assert.Equal("Selected", vm.Title);
        Assert.Equal("Description", vm.Description);
        Assert.Equal(task.DueDate, vm.DueDate);
        Assert.True(vm.IsCompleted);
    }
}