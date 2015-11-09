using System;
using System.Collections.Generic;
using QuickGraph;

namespace Codartis.SoftVis.Diagramming.Layout.Incremental.Relative
{
    /// <summary>
    /// Provides a read-only view of a layout graph.
    /// </summary>
    internal interface IReadOnlyLayoutGraph<TVertex, TEdge> : IBidirectionalGraph<TVertex, TEdge>
        where TEdge: IEdge<TVertex>
    {
        IEnumerable<TVertex> GetParents(TVertex vertex);
        IEnumerable<TVertex> GetChildren(TVertex vertex);
        void ExecuteOnDescendantVertices(TVertex rootVertex, Action<TVertex> actionOnVertex);
    }
}