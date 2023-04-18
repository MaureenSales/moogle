namespace MoogleEngine;


public static class Moogle
{
    public static SearchResult Query(string query) {
        Engine e = new Engine("../Content");

        return e.Query(query);
    }
}
