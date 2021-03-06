using System;
using System.Collections.Generic;
using Codartis.Util;
using JetBrains.Annotations;

namespace Codartis.SoftVis.Graphs.Immutable
{
    /// <summary>
    /// Defines the operations of an immutable bidirectional graph.
    /// All mutators return a new instance of the graph.
    /// All selectors treat vertex/edge equality as ID equality.
    /// </summary>
    /// <typeparam name="TVertex">The type of the vertices. Must be immutable.</typeparam>
    /// <typeparam name="TVertexId">The type of the vertex id.</typeparam>
    /// <typeparam name="TEdge">The type of the edges. Must be immutable.</typeparam>
    /// <typeparam name="TEdgeId">The type of the edge id.</typeparam>
    /// <remarks>
    /// WARNING: use only immutable types as TVertex and TEdge!
    /// TVertex and TEdge must have a unique, stable ID that identifies instances across versions.
    /// </remarks>
    public interface IImmutableBidirectionalGraph<TVertex, in TVertexId, TEdge, in TEdgeId>
        where TVertex : IImmutableVertex<TVertexId>
        where TVertexId : IEquatable<TVertexId>
        where TEdge : IImmutableEdge<TVertexId, TEdge, TEdgeId>
        where TEdgeId : IEquatable<TEdgeId>
    {
        IEnumerable<TVertex> Vertices { get; }
        IEnumerable<TEdge> Edges { get; }

        bool ContainsVertex(TVertexId vertexId);
        bool ContainsEdge(TEdgeId edgeId);

        IEnumerable<TEdge> GetInEdges(TVertexId vertexId);
        IEnumerable<TEdge> GetOutEdges(TVertexId vertexId);
        IEnumerable<TEdge> GetAllEdges(TVertexId vertexId);

        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> AddVertex(TVertex vertex);
        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> UpdateVertex(TVertex newVertex);
        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> RemoveVertex(TVertexId vertexId);

        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> AddEdge(TEdge edge);
        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> UpdateEdge(TEdge newEdge);
        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> RemoveEdge(TEdgeId edgeId);

        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> Clear();

        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> UpdateVertices([NotNull] Func<TVertex, TVertex> vertexMutatorFunc);

        IImmutableBidirectionalGraph<TVertex, TVertexId, TEdge, TEdgeId> UpdateVertices(
            [NotNull] Func<TVertex, bool> shouldUpdatePredicate,
            [NotNull] Func<TVertex, TVertex> vertexMutatorFunc);

        TVertex GetVertex(TVertexId vertexId);
        Maybe<TVertex> TryGetVertex(TVertexId vertexId);
        TEdge GetEdge(TEdgeId edgeId);
        Maybe<TEdge> TryGetEdge(TEdgeId edgeId);

        bool PathExists(TVertexId sourceVertexId, TVertexId targetVertexId);

        IEnumerable<TVertex> GetAdjacentVertices(
            TVertexId vertexId,
            EdgeDirection direction,
            Predicate<TEdge> edgePredicate = null,
            bool recursive = false);

        bool IsEdgeRedundant(TEdgeId edgeId);
    }
}