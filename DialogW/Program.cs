using DialogW;
using System;

public static class FileReaderService
{
    public static async Task DoAsync(Dialogue dialogue)
    {
        DialogueWritter writter = new DialogueWritter();
        writter.ActiveDialogue = dialogue;
        writter.OnParagraphFinalized += (Paragraph paragraph, string paragraphContent) =>
        {
            foreach (string block in SentenceSplitter.SplitParagraph(paragraphContent))
            {
                Console.WriteLine($" >> {block}");
                paragraph.Sentences.Add(block);
            }
        };

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
            await Task.Delay(500);
        }
        writter.FinalizeWritting();
        
        Console.WriteLine();

        Console.WriteLine(writter.ActiveDialogue);
    }
}

public static class Program
{
    public static async Task Main()
    {
        Dialogue dialogue = new Dialogue();
        await FileReaderService.DoAsync(dialogue);
    }
}
