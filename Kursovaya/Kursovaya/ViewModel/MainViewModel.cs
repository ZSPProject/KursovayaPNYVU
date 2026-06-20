using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Data;
using System.Windows.Input;
using Kursovaya.Commands;
using Kursovaya.Model;
using TaskModel = Kursovaya.Model.Task;
using System.Windows;

namespace Kursovaya.ViewModel;

public class MainViewModel : INotifyPropertyChanged
{
    private readonly TaskRepository _repository;

    public ObservableCollection<TaskModel> Tasks { get; }

    public ICollectionView TasksView { get; }

    private TaskModel? _selectedTask;

    public TaskModel? SelectedTask
    {
        get => _selectedTask;
        set
        {
            _selectedTask = value;

            if (value != null)
            {
                Title = value.Title;
                Description = value.Description;
                DueDate = value.DueDate;
                IsCompleted = value.IsCompleted;
            }

            OnPropertyChanged();
        }
    }

    private string _title = "";

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    private string _description = "";

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    private DateTime _dueDate = DateTime.Now;

    public DateTime DueDate
    {
        get => _dueDate;
        set
        {
            _dueDate = value;
            OnPropertyChanged();
        }
    }

    private bool _isCompleted;

    public bool IsCompleted
    {
        get => _isCompleted;
        set
        {
            _isCompleted = value;
            OnPropertyChanged();
        }
    }

    private string _selectedFilter = "Все";

    public string SelectedFilter
    {
        get => _selectedFilter;
        set
        {
            _selectedFilter = value;
            OnPropertyChanged();

            TasksView.Refresh();
        }
    }

    public ICommand AddTaskCommand { get; }
    public ICommand UpdateTaskCommand { get; }
    public ICommand DeleteTaskCommand { get; }
    public ICommand SaveCommand { get; }
    public ICommand SortByDateCommand { get; }
    public ICommand ExportCsvCommand { get; }

    public MainViewModel()
    {
        _repository = new TaskRepository();

        _repository.Load();

        Tasks = new ObservableCollection<TaskModel>(
            _repository.GetTasks());

        TasksView = CollectionViewSource.GetDefaultView(Tasks);

        TasksView.Filter = FilterTasks;

        AddTaskCommand = new RelayCommand(_ => AddTask());
        UpdateTaskCommand = new RelayCommand(_ => UpdateTask());
        DeleteTaskCommand = new RelayCommand(_ => DeleteTask());
        SaveCommand = new RelayCommand(_ => SaveTasks());
        SortByDateCommand = new RelayCommand(_ => SortTasks());
        ExportCsvCommand = new RelayCommand(_ => ExportCsv());
    }

    private bool FilterTasks(object obj)
    {
        if (obj is not TaskModel task)
            return false;

        return SelectedFilter switch
        {
            "Выполненные" => task.IsCompleted,
            "Невыполненные" => !task.IsCompleted,
            "Просроченные" =>
                !task.IsCompleted &&
                task.DueDate.Date < DateTime.Now.Date,
            _ => true
        };
    }

    private void AddTask()
    {
        if (string.IsNullOrWhiteSpace(Title))
            return;

        var task = new TaskModel
        {
            Title = Title,
            Description = Description,
            DueDate = DueDate,
            IsCompleted = IsCompleted
        };

        Tasks.Add(task);

        _repository.AddTask(task);
    }

    private void UpdateTask()
    {
        if (SelectedTask == null)
            return;

        SelectedTask.Title = Title;
        SelectedTask.Description = Description;
        SelectedTask.DueDate = DueDate;
        SelectedTask.IsCompleted = IsCompleted;

        _repository.UpdateTask(SelectedTask);

        TasksView.Refresh();
    }

    private void DeleteTask()
    {
        if (SelectedTask == null)
            return;
        _repository.RemoveTask(SelectedTask);
        Tasks.Remove(SelectedTask);
        _repository.Save();
    }

    private void SortTasks()
    {
        var sorted = Tasks
            .OrderBy(x => x.DueDate)
            .ToList();

        Tasks.Clear();

        foreach (var task in sorted)
            Tasks.Add(task);
    }

    private void ExportCsv()
    {
        string path = _repository.ExportCsv();

        MessageBox.Show(
            $"CSV-файл сохранён:\n{path}",
            "Экспорт завершён",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    public void SaveTasks()
    {
        _repository.Save();
    }

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(
        [CallerMemberName] string? name = null)
    {
        PropertyChanged?.Invoke(
            this,
            new PropertyChangedEventArgs(name));
    }
}