using Newtonsoft.Json;
using System.IO;
using System.Text;
namespace Kursovaya.Model;

public class TaskRepository
{
    private List<Task> _tasks = new();

    private readonly string _dataFolder;
    private readonly string _jsonPath;

    public TaskRepository()
    {
        _dataFolder = Path.Combine(
            Environment.GetFolderPath(
                Environment.SpecialFolder.UserProfile),
            "Kursovaya",
            "Data");

        Directory.CreateDirectory(_dataFolder);

        _jsonPath = Path.Combine(_dataFolder, "tasks.json");
    }

    public void AddTask(Task task)
    {
        _tasks.Add(task);
    }

    public bool UpdateTask(Task task)
    {
        var index = _tasks.FindIndex(x => x.Id == task.Id);

        if (index < 0)
            return false;

        _tasks[index] = task;

        return true;
    }

    public bool RemoveTask(Task task)
    {
        return _tasks.Remove(task);
    }

    public List<Task> GetTasks()
    {
        return _tasks.ToList();
    }

    public void Save()
    {
        var json = JsonConvert.SerializeObject(
            _tasks,
            Formatting.Indented);

        File.WriteAllText(_jsonPath, json);
    }

    public void Load()
    {
        if (!File.Exists(_jsonPath))
            return;

        var json = File.ReadAllText(_jsonPath);

        _tasks = JsonConvert.DeserializeObject<List<Task>>(json)
                 ?? new List<Task>();
    }

    public string ExportCsv()
    {
        string csvPath = Path.Combine(_dataFolder, "tasks.csv");

        using var writer = new StreamWriter(
            csvPath,
            false,
            new UTF8Encoding(true));

        writer.WriteLine("Id;Название;Описание;Срок выполнения;Выполнено");

        foreach (var task in _tasks)
        {
            writer.WriteLine(
                $"{task.Id};" +
                $"\"{task.Title}\";" +
                $"\"{task.Description}\";" +
                $"\"{task.DueDate:dd.MM.yyyy}\";" +
                $"{task.IsCompleted}");
        }

        return csvPath;
    }
}