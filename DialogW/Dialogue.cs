public class Dialogue
{
    public bool IsCompleted { get; private set; }
    public List<Sentence> Sentences { get; private set; } = [];

    public void MarkAsCompleted()
    {
        IsCompleted = true;
    }

    public override string ToString()
    {
        string dialogueState = IsCompleted ? "Completed" : "In Progress";
        string output = $"[DIALOGUE | {dialogueState}]\n";

        foreach (Sentence sentence in Sentences)
        {
            output += $"{sentence}\n\n";
        }

        return output;
    }
}
