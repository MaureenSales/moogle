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
        string[] query_tokenize = query.ToLower().Split(' ');
        return query_tokenize;
    }
    double[] QueryTf(string[] query)
    {
        Dictionary<string, double> tf = new Dictionary<string, double>();
        foreach (var word in query)
        {
            if (!tf.Keys.Contains(word))
            {
                tf[word] = 1f;
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
    double[] Vectorial_Model(double[,] documents, double[] query)
    {
        double[] result = new double[documents.GetLength(1)];
        for (int i = 0; i < documents.GetLength(1); i++)
        {
            double scalar_product = 0f;
            double mdl_d = 0f;
            double mdl_q = 0f;
            for (int j = 0; j < documents.GetLength(0); j++)
            {
                scalar_product += documents[j, i] * query[j];
                mdl_d += documents[j, i] * documents[j, i];
                mdl_q += query[j] * query[j];
            }
            result[i] = mdl_d * mdl_q == 0 ? 0f : scalar_product / (double)Math.Sqrt(mdl_d * mdl_q);
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
        double[] idfs = Folder.IDFS(Query);
        double[,] tfs = Folder.TFS(Query);
        double[,] documents = Build_Documents_Vectors(tfs, idfs);
        double[] query_vector = QueryTf(Query);
        double[] scores = Vectorial_Model(documents, query_vector);
        List<SearchItem> items = new List<SearchItem>();
        for (int i = 0; i < scores.Length; i++)
            if (scores[i] > 0f)
            {
                items.Add(new SearchItem(Folder.Files[i].Name, Snippet(Folder.Files[i].Root), (float)scores[i]));
                Console.WriteLine($"{Folder.Files[i].Name} {scores[i]}");
            }
        return new SearchResult(Utils.Sort(items.ToArray()), query);
    }
}