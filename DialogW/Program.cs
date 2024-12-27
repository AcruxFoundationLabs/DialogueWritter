using DialogW;
using System;

public static class FileReaderService
{
    public static async Task DoAsync(Dialogue dialogue)
    {
        DialogueWritter writter = new DialogueWritter();
        writter.ActiveDialogue = dialogue;
        writter.OnSentenceFinalized += (Sentence sentence) => { };

        string fullText = File.ReadAllText("text.txt").Replace("\r\n", "\n");
        string textSlice = "";
        for(int i = 0; i < fullText.Length; ++i)
        {
            textSlice += fullText[i];
			writter.UpdateContent(textSlice);
			await Task.Delay(20);
		}
		writter.FinalizeWritting();

		Console.WriteLine("\n\n");
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
