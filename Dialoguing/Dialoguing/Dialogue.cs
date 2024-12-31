namespace Acrux.Dialoguing;

public class Dialogue
{
	#region Sentences
	private List<Sentence> _Sentences { get; } = [];
    public IReadOnlyList<Sentence> Sentences => _Sentences.AsReadOnly();

	public event EventHandler? SentenceAdded;

	public void AddSentence(Sentence sentence)
	{
		_Sentences.Add(sentence);
		SentenceAdded?.Invoke(this, EventArgs.Empty);
	}
	#endregion

	#region Completation
	public bool IsCompleted { get; private set; }

	public void MarkAsCompleted()
	{
		IsCompleted = true;
	}
	#endregion

	public override string ToString()
    {
        string dialogueState = IsCompleted ? "Completed" : "In Progress";
        string output = $"[DIALOGUE | {dialogueState}]\n";

        foreach (Sentence sentence in _Sentences)
        {
            output += $"{sentence}\n\n";
        }

        return output;
    }
}
