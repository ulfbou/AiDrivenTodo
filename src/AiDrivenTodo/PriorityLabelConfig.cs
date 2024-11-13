using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace AiDrivenTodo;

public class PriorityLabelConfig
{
    public Dictionary<string, List<string>> Labels { get; set; }

    public static PriorityLabelConfig Load(string filePath)
    {
        var json = File.ReadAllText(filePath);
        return JsonSerializer.Deserialize<PriorityLabelConfig>(json);
    }
}
