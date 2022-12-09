using Icris.GremlinQuery;

using Play.Domain;
using Play.Domain.Events;

namespace Play.Persistence.Gremlin;

public record PersistenceObjects
{
    #region Instance Values

    public Vertex In { get; set; } = null!;
    public Vertex Out { get; set; } = null!;
    public Edge Edge { get; set; } = null!;

    #endregion

    #region Instance Members

    public static PersistenceObjects Create(DomainEvent domainEvent)
    {
        Vertex inVertex = Vertex.Create(domainEvent.Target!.AsDto());
        Vertex outVertex = Vertex.Create(domainEvent.Source!.AsDto());
        Edge edge = Edge.Create(inVertex, outVertex, domainEvent);

        return new PersistenceObjects
        {
            In = inVertex,
            Out = outVertex,
            Edge = edge
        };
    }

    private void Persist(object cosmosApi, PersistenceObjects persistenceObjects)
    {
        IVertexResult inVertex = CreateVertexResult(persistenceObjects.In).addE(persistenceObjects.Edge.Label);
        IVertexResult outVertex = CreateVertexResult(persistenceObjects.Out);
        IEdgeResult edgeResult = AddEdge(inVertex, outVertex, persistenceObjects.Edge);

        // persist
        // cosmosApi.Commit(inVertex);
        // cosmosApi.Commit(outVertex);
        // cosmosApi.Commit(edgeResult);
        throw new NotImplementedException();
    }

    private string Persist()
    {
        // TODO: Can we combine this string query or we need to persist separately?
        IVertexResult inVertex = CreateVertexResult(In).addE(Edge.Label);
        IVertexResult outVertex = CreateVertexResult(Out);
        IEdgeResult edgeResult = AddEdge(inVertex, outVertex, Edge);

        foreach (var nestedInObject in In.NestedObjects)

            // persist nested In objects
            Persist(null, nestedInObject);

        foreach (var nestedOutObject in Out.NestedObjects)

            // persist nested In objects
            Persist(null, nestedOutObject);

        throw new NotImplementedException();
    }

    internal static PersistenceObjects Create(IDto source, IDto target)
    {
        Vertex sourceVertex = Vertex.Create(source);
        var targetVertex = Vertex.Create(target);
        var edge = new Edge()
        {
            Label = "HasA",
            In = targetVertex,
            Out = sourceVertex
        };

        return new PersistenceObjects()
        {
            Edge = edge,
            In = targetVertex,
            Out = sourceVertex
        };
    }

    private static IVertexResult CreateVertexResult(Vertex vertex)
    {
        IVertexResult result = g.addV(vertex.Label);
        foreach (var property in vertex.Properties)
            result = result.property(property.Name, property.Value);

        return result;
    }

    private static IEdgeResult AddEdge(IVertexResult inVertex, IVertexResult outVertex, Edge edge)
    {
        var edgeResult = inVertex.addE(edge.Label);

        foreach (var property in edge.Properties)
            edgeResult.property(property.Name, property.Value);

        return edgeResult.to(outVertex);
    }

    #endregion
}