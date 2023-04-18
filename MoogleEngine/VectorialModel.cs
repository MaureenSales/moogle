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
    float[] QueryTf(string[] query)
    {
        Dictionary<string,float> tf = new Dictionary<string, float>();
        foreach(var word in query)
        {
            if(!tf.Keys.Contains(word))
            {
                tf[word] = 1f;
            }
            else
            {
                tf[word]++;
            }
        }
        float[] result = new float[tf.Keys.Count];
        for(int i = 0; i < result.Length; i++)
            result[i] = tf[tf.Keys.ToArray()[i]];
        return result;
    }
    float[] Query_Vector(float[] tf, float[] idf, float value)
    {
        for(int i = 0; i < tf.Length; i++)
            tf[i] = tf[i] * idf[i] / tf.Length;
        return tf;
    }
    float[,] Build_Documents_Vectors(float[,] tfs, float[]idfs)
    {
        for(int i = 0; i < tfs.GetLength(0); i++)
            for(int j = 0; j < tfs.GetLength(1); j++)
                tfs[i,j] *= idfs[i];
        return tfs;
    }
    float[] Vectorial_Model(float[,] documents, float[] query)
    {
        float[] result = new float[documents.GetLength(1)];
        for(int i = 0; i < documents.GetLength(1); i++)
        {
            float scalar_product = 0f;
            float mdl_d = 0f;
            float mdl_q = 0f;
            for(int j = 0; j < documents.GetLength(0); j++)
            {
                scalar_product += documents[j,i] * query[j];
                mdl_d += documents[j,i] * documents[j,i];
                mdl_q += query[j] * query[j];
            }
            result[i] = mdl_d * mdl_q == 0 ? 0f : scalar_product / (float)Math.Sqrt(mdl_d *mdl_q); 
        }
        return result;
    }
    public string Snippet(string document)
    {
        StreamReader reader = new StreamReader(document);
        string snippet = reader.ReadToEnd();
        reader.Close();
        return snippet.Substring(0,Math.Min(500,snippet.Length));
    }
    public SearchResult Query(string query)
    {
        string[] Query = TokenizeQuery(query.ToLower());
        float[] idfs = Folder.IDFS(Query);
        float[,] tfs = Folder.TFS(Query);
        float[,] documents = Build_Documents_Vectors(tfs,idfs);
        float[] query_vector = Query_Vector(QueryTf(Query),idfs,0.4f);
        float[] scores = Vectorial_Model(documents,query_vector);
        List<SearchItem> items = new List<SearchItem>();
        for(int i = 0; i < scores.Length; i++)
            if(scores[i] > 0f) {
                items.Add(new SearchItem(Folder.Files[i].Name,Snippet(Folder.Files[i].Root),scores[i]));
                Console.WriteLine($"{Folder.Files[i].Name} {scores[i]}");
            }
        return new SearchResult(Utils.Sort(items.ToArray()),query);
    }
}