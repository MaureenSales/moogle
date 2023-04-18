namespace MoogleEngine;

public class DataFile
{
    //nombre del documento
    public string Name { get; private set; }
    //ruta del documento
    public string Root {get; private set; }
    //diccionario con el tf de cada palabra e el documento
    Dictionary<string,float> tfs { get; set; }
    //maximo tf encontrado
    float NumWords{ get; set; }
    public DataFile(string Root)
    {
        System.Console.Write("Loading document " + Root + " .....");
        //guardamos la ruta
        this.Root = Root;
        //creamos el diccionario
        tfs = new Dictionary<string, float>();
        //extraemos el nombre del documento
        int last_apparicion = Root.LastIndexOf('\\');
        Name = Root.Substring(last_apparicion + 1);
        //leemos el documento
        StreamReader reader = new StreamReader(Root);
        string content = reader.ReadToEnd().ToLower();
        //lo separamos por palabras
        reader.Close();
        string[] words = content.Split(' ',',',';','.',':','\n');
        foreach(var word in words)
        {
            if(tfs.Keys.Contains(word))
            {
                tfs[word]++;
            }
            else
            {
                tfs[word] = 1;
            }
        }
        this.NumWords = words.Length;
        System.Console.Write("Done\n");
    }
    public float this[string word]
    {
        get
        {
            //si el diccionario contiene a la palabra dada entre sus llaves devolvemos el tf
            if(tfs.Keys.Contains(word))
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