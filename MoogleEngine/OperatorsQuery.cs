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

    public static double RemoveWords(List<string> not, Dictionary<string, double> tfs)
    {
        foreach (var word in not)
        {
            if (tfs.Keys.Contains(word))
            {
                return 0f;
            }

        }
        return 1f;
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

    public static double NeedWords(List<string> yes, Dictionary<string, double> tfs)
    {
        foreach (var word in yes)
        {
            if (!tfs.Keys.Contains(word))
            {
                return 0f;
            }

        }
        return 1f;
    }

    public static Dictionary<string, int> WordsPriority(string query)
    {
        Dictionary<string, int> words_priority = new Dictionary<string, int>();
        query += " ";
        int first = query.IndexOf("*");
        while (query.IndexOf("*") >= 0)
        {
            int count_ast = 0;
            for (int i = first; i < query.Length; i++)
            {
                if (query[i] == '*')
                {
                    count_ast++;
                }
                else
                {
                    int space = query.IndexOf(" ", i);
                    string word = query.Substring(i, space - i);
                    words_priority[word] = count_ast;
                    query = query.Substring(space);
                    first = query.IndexOf("*");

                    break;
                }
            }

        }
        return words_priority;
    }
    
}