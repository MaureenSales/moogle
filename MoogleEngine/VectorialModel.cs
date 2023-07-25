namespace MoogleEngine;

public class Engine
{
    DataFolder Folder { get; set; }
    public Engine(string folder)
    {
        Folder = new DataFolder(folder);
    }
    string[] TokenizeQuery(string query)
    {
        char[] sg = { ' ', ',', ';', '.', ':', '\n', '\t' };
        string[] query_tokenize = query.ToLower().Split(sg, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < query_tokenize.Length; i++)
        {
            query_tokenize[i] = Utils.TokenizeWord(query_tokenize[i]);
        }
        return query_tokenize;
    }

    double[] QueryTf(string[] query, Dictionary<string, int> priority)
    {
        Dictionary<string, double> tf = new Dictionary<string, double>();
        foreach (var word in query)
        {
            if (!tf.Keys.Contains(word))
            {
                tf[word] = 1f;
            }

        }

        foreach (var x in priority)
        {
            if (!tf.Keys.Contains(x.Key))
            {
                tf[x.Key] = 2 * x.Value;
            }
            else
            {
                tf[x.Key] += 2 * x.Value;
            }
        }

        double[] result = new double[tf.Keys.Count];
        for (int i = 0; i < result.Length; i++)
            result[i] = tf[tf.Keys.ToArray()[i]] / query.Length;
        return result;
    }
    double[,] Build_Documents_Vectors(double[,] tfs, double[] idfs)
    {
        for (int i = 0; i < tfs.GetLength(0); i++)
            for (int j = 0; j < tfs.GetLength(1); j++)
                tfs[i, j] *= idfs[j];
        return tfs;
    }

    double Query_Norm(double[] query_vector)
    {
        double norm = 0.0;
        for (int i = 0; i < query_vector.Length; i++)
        {
            double v = Math.Min(1, query_vector[i]);
            norm += v * v;
        }
        return Math.Sqrt(norm);
    }
    double[] Vectorial_Model(double[,] documents, double[] query_vector)
    {
        double[] result = new double[documents.GetLength(0)];
        for (int i = 0; i < documents.GetLength(0); i++)
        {
            double numerator = 0.0;
            double denominator = 0.0;
            for (int j = 0; j < documents.GetLength(1); j++)
            {
                numerator += documents[i, j] * query_vector[j];
            }

            denominator = Folder.norma[i] * Query_Norm(query_vector);

            if (denominator != 0)
            {
                result[i] = numerator / denominator;
            }
            else
            {
                result[i] = 0.0;
            }
        }

        return result;
    }
    public string Snippet(string document)
    {
        StreamReader reader = new StreamReader(document);
        string snippet = reader.ReadToEnd();
        reader.Close();
        return snippet.Substring(0, Math.Min(500, snippet.Length));
    }
    public SearchResult Query(string query)
    {
        string[] Query = TokenizeQuery(query.ToLower());
        List<string> need = OperatorsQuery.WordsYes(query.ToLower());
        List<string> not = OperatorsQuery.WordsNot(query.ToLower());
        Dictionary<string, int> priority = OperatorsQuery.WordsPriority(query.ToLower());
        double[] idfs = Folder.IDFS(Query);
        double[,] tfs = Folder.TFS(Query);
        double[,] documents = Build_Documents_Vectors(tfs, idfs);
        double[] query_vector = QueryTf(Query, priority);
        double[] scores = Vectorial_Model(documents, query_vector);
        List<SearchItem> items = new List<SearchItem>();

        for (int i = 0; i < scores.Length; i++)
        {
            var t = Folder.Files[i].tfs;
            if (scores[i] * OperatorsQuery.NeedWords(need, t) * OperatorsQuery.RemoveWords(not, t) > 0f)
            {
                items.Add(new SearchItem(Folder.Files[i].Name, Snippet(Folder.Files[i].Root), (float)scores[i]));
            }
        }
        return new SearchResult(Utils.Sort(items.ToArray()), query);
    }
}