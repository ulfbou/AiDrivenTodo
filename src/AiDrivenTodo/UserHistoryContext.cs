using System.Collections.Generic;

public class UserHistoryContext
{
    private readonly Dictionary<string, string> _priorityPreferences = new();

    public void RecordUserAdjustment(string taskDescription, string newPriorityLevel)
    {
        _priorityPreferences[taskDescription] = newPriorityLevel;
    }

    public string? GetUserPreferredPriority(string taskDescription)
    {
        return _priorityPreferences.ContainsKey(taskDescription) ? _priorityPreferences[taskDescription] : null;
    }
}