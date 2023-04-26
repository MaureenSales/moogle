namespace MoogleEngine;

public static class OperatorsQuery
{
    public static List<string> WordsNot(string query)
    {
        List<string> not = new List<string>();
        query = query + " ";
        while (query.IndexOf("!") >= 0)
        {
            int startIndex = query.IndexOf("!") + 1;
            string part_query = query.Substring(startIndex);
            int endIndex = part_query.IndexOf(" ");
            not.Add(part_query.Substring(0, endIndex));
            query = part_query.Substring(endIndex + 1);
        }
        return not;
    }

    public static float[] RemoveWords(List<string> not, Dictionary<string, float> tfs)
    {
        float[] remove = new float[tfs.Keys.Count];
        int i = 0;
        foreach (var word in not)
        {
            if (tfs.Keys.Contains(word))
            {
                remove[i] = 0f;
            }
            else
            {
                remove[i] = 1f;
            }
            i++;
        }
        return remove;
    }
    public static List<string> WordsYes(string query)
    {
        List<string> yes = new List<string>();
        query = query + " ";
        while (query.IndexOf("^") >= 0)
        {
            int startIndex = query.IndexOf("^") + 1;
            string part_query = query.Substring(startIndex);
            int endIndex = part_query.IndexOf(" ");
            yes.Add(part_query.Substring(0, endIndex));
            query = part_query.Substring(endIndex + 1);
        }
        return yes;
    }

    public static float[] NeedWords(List<string> yes, Dictionary<string, float> tfs)
    {
        float[] need = new float[tfs.Keys.Count];
        int i = 0;
        foreach (var word in yes)
        {
            if (!tfs.Keys.Contains(word))
            {
                need[i] = 0f;
            }
            else
            {
                need[i] = 1f;
            }
            i++;
        }
        return need;
    }



}