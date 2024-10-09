public class Paragraph
{
    public bool IsCompleted { get; private set; }
    public string Content { get; set; } = "";

    public Paragraph(string initialContent)
    {
        Console.WriteLine("\n\n== NEW PARAGRAPH ==");
        Content = initialContent;
        Console.Write(initialContent);
    }

    public void MarkAsCompleted()
    {
        IsCompleted = true;
        Console.WriteLine("\n=== PARAGRAPH COMPLETED ===");
    }

    public void AppendToContent(string additionalContent)
    {
        Content += additionalContent;
        Console.Write(additionalContent);
    }

    public override string ToString()
    {
        string paragraphState = IsCompleted ? "Completed" : "In Progress";
        string output = $"[PARAGRAPH | {paragraphState}]\n{Content}";
        return output;
    }
}
