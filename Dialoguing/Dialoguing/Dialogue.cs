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

	#region Traverse
	public int CurrentSentenceIndex { get; private set; } = -1;
	public Sentence CurrentSentence => _Sentences[CurrentSentenceIndex];

	/// <summary>
	/// Whether the <see cref="CurrentSentence"/> represents the
	/// final <see cref="Sentence"/> of this <see cref="Dialogue"/>.
	/// <para>If the dialogue is not completed, returns <see langword="false"/>.</para>
	/// </summary>
	public bool IsCurrentSentenceFinal =>
		CurrentSentenceIndex == _Sentences.Count-1 &&
		IsCompleted;

	/// <summary>
	/// Wheter there's a <see cref="Sentence"/> following the current one and if it's completed.
	/// </summary>
	public bool CanMoveNext =>
		!IsCurrentSentenceFinal &&
		CurrentSentenceIndex < _Sentences.Count &&
		_Sentences.Last().IsCompleted;

	/// <summary>
	/// Wheter there's a <see cref="Sentence"/> before the current one.
	/// </summary>
	public bool CanMovePrevious =>
		CurrentSentenceIndex > 0;

	public event EventHandler? MovedNext;
	public event EventHandler? MovedPrevious;

	public Sentence NextSentence()
	{
		if (!CanMoveNext)
			throw new InvalidOperationException("Cannot move to the next sentence.");

		CurrentSentenceIndex++;
		MovedNext?.Invoke(this, EventArgs.Empty);
		return CurrentSentence;
	}

	public Sentence PreviousSentence()
	{
		if (!CanMovePrevious)
			throw new InvalidOperationException("Cannot move to the previous sentence.");

		CurrentSentenceIndex--;
		MovedPrevious?.Invoke(this, EventArgs.Empty);
		return CurrentSentence;
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
