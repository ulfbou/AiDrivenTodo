using AiDrivenTodo;

var taskClassifier = new TaskClassification();

var tasks = new List<TaskItem> {
    new TaskItem { Description = "Complete project report by tomorrow", DueDate = DateTime.Today.AddDays(1) },
    new TaskItem { Description = "Organize meeting with team", DueDate = DateTime.Today.AddDays(2) },
    new TaskItem { Description = "Clean workspace", DueDate = DateTime.Today.AddDays(7) }
};

foreach (var task in tasks)
{
    task.PriorityLevel = taskClassifier.PredictPriority(task.Description);
    if (string.IsNullOrEmpty(task.PriorityLevel))
    {
        Console.WriteLine($"Unable to predict priority for task: {task.Description}");
        continue;
    }
    Console.WriteLine($"{task.Description} - Priority: {task.PriorityLevel}");
}