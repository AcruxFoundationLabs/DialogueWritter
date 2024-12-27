using DialogW;
using DialogW.Events;

/// <summary>
/// Modifies a <see cref="Dialogue"/>'s instance by calling <see cref="UpdateContent(string)"/>
/// where <c>newContent</c> starts with the last given one <br></br>
/// (stored in <see cref="Content"/>),
/// In other words, <c>newContent</c> is an extension of the last given one.
/// </summary>
public class DialogueWritter
{
    /// <summary>
    /// Contains the computed sentences from the last given <c>newContent</c> argument
    /// in the <see cref="UpdateContent(string)"/> method.
    /// </summary>
    private string[] CurrentSentences { get; set; } = Array.Empty<string>();

    /// <summary>
    /// Contains the last given <c>newContent</c> argument in the <see cref="UpdateContent(string)"/> method.
    /// </summary>
    private string? Content { get; set; } = null;

    /// <summary>
    /// The <see cref="Dialogue"/> being handled by this <see cref="DialogueWritter"/>.
    /// </summary>
    public Dialogue ActiveDialogue { get; set; } = new Dialogue();

    /// <summary>
    /// Invoked everytime a sentence is finalized.
    /// </summary>
    public event SentenceFinalizedEventHandler? OnSentenceFinalized;

    /// <summary>
    /// Modifies this <see cref="ActiveDialogue"/> via the given <paramref name="newContent"/>.<br></br><br></br>
    /// Throws <see cref="ArgumentException"/> if the given <paramref name="newContent"/> is not an extension of the last given one.
    /// </summary>
    /// <param name="newContent">Should be an extension of the previous given one.</param>
    /// <exception cref="ArgumentException"></exception>
    public void UpdateContent(string newContent)
    {
        if (Content != null && !newContent.StartsWith(Content))
        {
            throw new ArgumentException($"The provided {nameof(newContent)} should be an extension of the previously given content.", nameof(newContent));
        }

        // Split content into sentences.
        string[] newSentences = DialogueSplitter.SplitIntoSentences(newContent);

        // Update existing sentences if content is modified.
        for (int i = 0; i < CurrentSentences.Length; ++i)
        {
            bool isSentenceModified = CurrentSentences[i] != newSentences[i];
            if (isSentenceModified)
            {
                int modifiedIndex = CurrentSentences[i].Length;
                ActiveDialogue.Sentences[i].AppendToContent(newSentences[i].Substring(modifiedIndex));
            }
        }

        // Handle newly added sentences.
        for (int i = CurrentSentences.Length; i < newSentences.Length; ++i)
        {
            if (ActiveDialogue.Sentences.Count > 0)
            {
                Sentence sentenceToMark = ActiveDialogue.Sentences[i - 1];
				sentenceToMark.MarkAsCompleted();
                OnSentenceFinalized?.Invoke(sentenceToMark);
            }
            ActiveDialogue.Sentences.Add(new Sentence(newSentences[i]));
        }

        CurrentSentences = newSentences;
        Content = newContent;
    }

    /// <summary>
    /// This should be called after ensuring <see cref="UpdateContent"/> will not be called again. <br></br>
    /// This function helps mark the remaining <see cref="Sentence"/>s and the <see cref="ActiveDialogue"/> as completed.
    /// </summary>
    public void FinalizeWritting()
    {
        //Mark remaining sentences as completed
        Sentence remainingSentence = ActiveDialogue.Sentences.Last();
		remainingSentence.MarkAsCompleted();
		OnSentenceFinalized?.Invoke(remainingSentence);

        //Mark dialogue as completed.
		ActiveDialogue.MarkAsCompleted();
	}
}
