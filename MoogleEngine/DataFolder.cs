namespace MoogleEngine;

public class DataFolder
{
    //array con las instancias de los archivos a leer
    public DataFile[] Files {get; private set; }
    //diccionario con el idf de cada palabra
    Dictionary<string,float> idfs { get; set; }
    public DataFolder(string Root)
    {
        //creamos el diccionario
        idfs = new Dictionary<string, float>();
        //obtenemos cada ruta a cada archivo
        string[] roots = Utils.EnumerateFiles(Root);
        //creamos el array
        Files = new DataFile[roots.Length];
        //leemos los documentos
        for(int i = 0; i < Files.Length; i++)
            Files[i] = new DataFile(roots[i]);
        foreach(var file in Files)
        {
            foreach(var word in file.Words)
            {
                if(idfs.Keys.Contains(word))
                    idfs[word]++;
                else
                    idfs[word] = 1;
            }
        }
        foreach(var key in idfs.Keys)
            idfs[key] = (float)Math.Log(Files.Length / idfs[key]);
    }
    public float[] IDFS(string[] words)
    {
        float[] result = new float[words.Length];
        for(int i = 0; i < words.Length; i++)
        {
            if(idfs.Keys.Contains(words[i]))
                result[i] = idfs[words[i]];
            else
                result[i] = -1f;
        }
        return result;
    }
    public float[,] TFS(string[] words)
    {
        float[,] result = new float[words.Length,Files.Length];
        for(int i = 0; i < words.Length; i++)
            for(int j = 0; j < Files.Length; j++)
                result[i,j] = Files[j][words[i]];
        return result;
    }
}