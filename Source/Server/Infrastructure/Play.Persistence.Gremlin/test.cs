using Icris.GremlinQuery;

namespace Play.Persistence.Gremlin;

public class test
{
    #region Instance Members

    public void Hello()
    {
        g.addV("person").property("name", "marko").property("age", 29).property("id", 1).ToString();
    }

    #endregion
}