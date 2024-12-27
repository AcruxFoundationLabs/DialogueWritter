using DialogW;

public class Sentence
{
    public bool IsCompleted { get; private set; }
    public string Content { get; set; } = "";

    public Sentence(string initialContent)
    {
        Console.WriteLine("\n=== NEW SENTENCE ===");
        Content = initialContent;
        Console.Write(initialContent);
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        Console.WriteLine("\n=== SENTENCE COMPLETED ===");
    }

    public void AppendToContent(string additionalContent)
    {
        Content += additionalContent;
		Console.Write(additionalContent);
    }

    public override string ToString()
    {
        string state = IsCompleted ? "Completed" : "In Progress";
        string output = $"[PARAGRAPH | {state}]\n{Content}";
        return output;
    }
}
