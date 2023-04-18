namespace MoogleEngine;


public static class Moogle
{
    public static Engine e = new Engine("../Content");
    public static SearchResult Query(string query) {
        return e.Query(query);
    }
}
