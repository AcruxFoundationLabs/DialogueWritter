public class Dialogue
{
    public bool IsCompleted { get; private set; }
    public List<Paragraph> Paragraphs { get; private set; } = [];

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
