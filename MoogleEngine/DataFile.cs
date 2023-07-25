namespace MoogleEngine;

public class DataFile
{
    //nombre del documento
    public string Name { get; private set; }
    //ruta del contenedor 
    public string Root { get; private set; }
    //diccionario con el tf de cada palabra e el documento
    public Dictionary<string, double> tfs { get; set; }
    //cantidad de palabras por documento
    double NumWords { get; set; }
    public DataFile(string Root)
    {
        System.Console.Write("Loading document " + Root + " .....");
        //guardamos la ruta
        this.Root = Root;
        //creamos el diccionario
        this.tfs = new Dictionary<string, double>();
        //extraemos el nombre del documento
        int last_apparicion = Root.LastIndexOf('\\');
        Name = Root.Substring(last_apparicion + 1);
        //leemos el documento
        StreamReader reader = new StreamReader(Root);
        string content = reader.ReadToEnd().ToLower();
        //lo separamos por palabras
        reader.Close();
        char[] sg = { ' ', ',', ';', '.', ':', '\n', '\t' };
        string[] words = content.Split(sg, StringSplitOptions.RemoveEmptyEntries);

        for (int i = 0; i < words.Length; i++)
        {
            words[i] = Utils.TokenizeWord(words[i]);
        }
        
        foreach (var word in words)
        {
            if (tfs.Keys.Contains(word))
            {
                tfs[word]++;
            }
            else
            {
                tfs[word] = 1;
            }
        }

        this.NumWords = words.Length;
        System.Console.Write($"Done\n ");
    }
    public double this[string word]
    {
        get
        {
            //si el diccionario contiene a la palabra dada entre sus llaves devolvemos el tf
            if (tfs.Keys.Contains(word))
                return tfs[word] / NumWords;
            //sino devolvemos 0
            return 0f;
        }
    }
    public string[] Words
    {
        get
        {
            return tfs.Keys.ToArray();
        }
    }
}