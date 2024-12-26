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
    /// Invoked everytime a paragraph is finalized.
    /// </summary>
    public event ParagraphFinalizedEventHandler? OnParagraphFinalized;

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
        string[] newParagraphs = SentenceSplitter.SplitIntoSentences(newContent);

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
                Paragraph paragraphToMark = ActiveDialogue.Paragraphs[i - 1];
				paragraphToMark.MarkAsCompleted();
                OnParagraphFinalized?.Invoke(paragraphToMark);
            }
            Paragraph newParagraph = new Paragraph(newParagraphs[i]);
            ActiveDialogue.Paragraphs.Add(newParagraph);
        }

        CurrentParagraphs = newParagraphs;
        Content = newContent;
    }

    /// <summary>
    /// This should be called after ensuring <see cref="UpdateContent"/> will not be called again. <br></br>
    /// This function helps mark the remaining <see cref="Paragraph"/> and the <see cref="ActiveDialogue"/> as completed.
    /// </summary>
    public void FinalizeWritting()
    {
        //Mark remaining paragraph as completed
        Paragraph remainingParagraph = ActiveDialogue.Paragraphs.Last();
		remainingParagraph.MarkAsCompleted();
		OnParagraphFinalized?.Invoke(remainingParagraph);

        //Mark dialogue as completed.
		ActiveDialogue.MarkAsCompleted();
	}
}
