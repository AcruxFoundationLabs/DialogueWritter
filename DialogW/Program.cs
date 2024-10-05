using System;

public class Dialogue
{
    public bool IsCompleted { get; private set; }
    public List<Paragraph> Paragraphs { get; set; } = [];

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    public override string ToString()
    {
        string dialogueState = IsCompleted ? "Completed" : "In Progress";
        string output = $"[DIALOGUE | {dialogueState}]\n";

        foreach (Paragraph paragraph in Paragraphs)
        {
            output += $"{paragraph}\n\n";
        }

        return output;
    }
}

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

/// <summary>
/// Modifies a <see cref="Dialogue"/>'s instance by calling <see cref="UpdateContent(string)"/>
/// where <c>newContent</c> starts with the last given one <br></br>
/// (stored in <see cref="Content"/>),
/// In other words, <c>newContent</c> is an extension of the last given one.
/// </summary>
public class DialogueWritter
{
    /// <summary>
    /// Contains the computed paragraphs from the last given <c>newContent</c> argument
    /// in the <see cref="UpdateContent(string)"/> method.
    /// </summary>
    private string[] CurrentParagraphs { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Contains the last given <c>newContent</c> argument in the <see cref="UpdateContent(string)"/> method.
    /// </summary>
    private string? Content { get; set; } = null;

    /// <summary>
    /// The <see cref="Dialogue"/> being handled by this <see cref="DialogueWritter"/>.
    /// </summary>
    public Dialogue ActiveDialogue { get; set; } = new Dialogue();

    /// <summary>
    /// Modifies this <see cref="ActiveDialogue"/> via the given <paramref name="newContent"/>.<br></br><br></br>
    /// Throws <see cref="ArgumentException"/> if the given <paramref name="newContent"/> is not an extension of the last given one.
    /// </summary>
    /// <param name="newContent">Should be an extension of the previous given one.</param>
    /// <exception cref="ArgumentException"></exception>
    public void UpdateContent(string newContent)
    {
        if (newContent.Length > 0 && newContent.EndsWith("\n") && !newContent.EndsWith("\n\n"))
        {
            return; // Handle single newline as paragraph continuation.
        }

        /* Ignore cases where theres a '\n' at the end of the `newContent`
         * because it may actually be part of a "\n\n" (Paragraph terminator) */
        if (Content != null && !newContent.StartsWith(Content))
        {
            throw new ArgumentException($"The provided {nameof(newContent)} should be an extension of the previously given content.", nameof(newContent));
        }

        // Split content into paragraphs.
        string[] newParagraphs = newContent.Split(new[] { "\n\n" }, StringSplitOptions.None);

        // Update existing paragraphs if content is modified.
        for (int i = 0; i < CurrentParagraphs.Length; ++i)
        {
            bool isParagraphModified = CurrentParagraphs[i] != newParagraphs[i];
            if (isParagraphModified)
            {
                int modifiedIndex = CurrentParagraphs[i].Length;
                ActiveDialogue.Paragraphs[i].AppendToContent(newParagraphs[i].Substring(modifiedIndex));
            }
        }

        // Handle newly added paragraphs.
        for (int i = CurrentParagraphs.Length; i < newParagraphs.Length; ++i)
        {
            if (ActiveDialogue.Paragraphs.Count > 0)
            {
                ActiveDialogue.Paragraphs[i - 1].MarkAsCompleted();
            }
            Paragraph newParagraph = new Paragraph(newParagraphs[i]);
            ActiveDialogue.Paragraphs.Add(newParagraph);
        }

        CurrentParagraphs = newParagraphs;
        Content = newContent;
    }
}

public static class FileReaderService
{
    public static async Task DoAsync(Dialogue dialogue)
    {
        DialogueWritter writter = new DialogueWritter();
        writter.ActiveDialogue = dialogue;

        string fullText = File.ReadAllText("text.txt").Replace("\r\n", "\n");
        string contentSlice = "";
        int currentIndex = 0;

        Random random = new Random();
        while (contentSlice != fullText)
        {
            int previousIndex = currentIndex;
            int increment = (random.Next(fullText.Length - currentIndex + 1) / 2);
            increment = (increment == 0) ? 1 : increment;
            currentIndex += increment;
            contentSlice += fullText.Substring(previousIndex, currentIndex - previousIndex);
            writter.UpdateContent(contentSlice);
            await Task.Delay(500); // Use await instead of Task.Delay
        }

        // Mark the entire dialogue and last paragraph as completed.
        writter.ActiveDialogue.MarkAsCompleted();
        writter.ActiveDialogue.Paragraphs.Last().MarkAsCompleted();
        Console.WriteLine();

        Console.WriteLine(writter.ActiveDialogue);
    }
}

public static class Program
{
    public static void Main()
    {
        Dialogue dialogue = new Dialogue();
        Task task = FileReaderService.DoAsync(dialogue);

        Console.WriteLine("\nDOING SOMETHING ELSE");
        while(!task.IsCompleted)
        {
            Thread.Sleep(1000);
        }

        Console.WriteLine("END");
    }
}
