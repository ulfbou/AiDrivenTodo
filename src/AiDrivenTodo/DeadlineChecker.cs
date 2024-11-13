namespace AiDrivenTodo;

public class DeadlineChecker
{
    public static string GetWarning(DateTime dueDate)
    {
        var daysRemaining = (dueDate - DateTime.Now).TotalDays;

        if (daysRemaining <= 1)
            return "Due Soon";
        if (daysRemaining <= 3)
            return "Approaching Deadline";
        return "On Track";
    }
}