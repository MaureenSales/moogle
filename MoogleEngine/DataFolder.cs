namespace MoogleEngine;

public class DataFolder
{
    //array con las instancias de los archivos a leer
    public DataFile[] Files { get; set; }
    //diccionario con el idf de cada palabra
    public Dictionary<string, double> idfs { get; set; }

    public double[] norma { get; set; }
    public DataFolder(string Root)
    {
        //creamos el diccionario
        this.idfs = new Dictionary<string, double>();
        //obtenemos cada ruta a cada archivo
        string[] roots = Utils.EnumerateFiles(Root);
        //creamos el array
        this.Files = new DataFile[roots.Length];
        //leemos los documentos
        this.norma = new double[Files.Length];
        //vectores normalizados de cada file
        for (int i = 0; i < Files.Length; i++)
            Files[i] = new DataFile(roots[i]);
        foreach (var file in Files)
        {
            foreach (var word in file.Words)
            {
                if (idfs.Keys.Contains(word))
                    idfs[word]++;
                else
                    idfs[word] = 1;
            }
        }
        // por cada doc en files acceder a las palabras y acceder a traves de ellas al tf, luego calc relv con dic idfs
        foreach (var key in idfs.Keys)
            idfs[key] = (double)Math.Log(Files.Length / idfs[key]);

    }

    public double[] IDFS(string[] words)
    {
        double[] result = new double[words.Length];
        for (int i = 0; i < words.Length; i++)
        {
            if (idfs.Keys.Contains(words[i]))
                result[i] = idfs[words[i]];
            else
                result[i] = 0f;
        }
        return result;
    }

    public double[,] TFS(string[] words)
    {
        double[,] result = new double[Files.Length, words.Length];
        for (int i = 0; i < Files.Length; i++)
            for (int j = 0; j < words.Length; j++)
                result[i, j] = Files[i][words[j]];
        return result;
    }

    public double[] Norm(double[] tfs, double[] idfs)
    {
        double[] norm = new double[Files.Length];
        for (int i = 0; i < tfs.Length; i++)
        {
            norm[i] += tfs[i] * idfs[i];

        }
        return norm;
    }
}