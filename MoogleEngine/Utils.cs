namespace MoogleEngine;

public static class Utils
{
    public static string[] EnumerateFiles(string root)
    {
        return Directory.EnumerateFiles(root).ToArray();
    }
    public static Engine LoadDocuments(string folder)
    {
        string documents_root = Directory.GetCurrentDirectory() + @"\" + folder;
        return new Engine(documents_root);
    }
    
    public static string TokenizeWord (string word)
    {
        string result = "";

        for (int i = 0; i < word.Length; i++)
        {
            if(Char.IsLetterOrDigit(word[i]))
            {
                result += word[i];
            }
        }
        return result;
    }
    public static SearchItem[] Sort(SearchItem[] items)
    {
        for (int i = 0; i < items.Length; i++)
            for (int j = i; j < items.Length; j++)
                if (items[j].Score > items[i].Score)
                {
                    SearchItem temp = items[j];
                    items[j] = items[i];
                    items[i] = temp;
                }
        return items;
    }
}