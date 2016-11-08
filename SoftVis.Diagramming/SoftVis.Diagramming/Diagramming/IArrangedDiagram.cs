﻿using System;
using Codartis.SoftVis.Geometry;

namespace Codartis.SoftVis.Diagramming
{
    /// <summary>
    /// A diagram with layout information.
    /// </summary>
    /// <remarks>
    /// When a node is added to the diagram it does not have size and position.
    /// </remarks>
    public interface IArrangedDiagram : IDiagram
    {
        Rect2D ContentRect { get; }

        void ResizeNode(IDiagramNode diagramNode, Size2D newSize);
        void MoveNodeCenter(IDiagramNode diagramNode, Point2D newCenter);
        void RerouteConnector(IDiagramConnector diagramConnector, Route newRoute);

        event Action<IDiagramNode, Size2D, Size2D> NodeSizeChanged;
        event Action<IDiagramNode, Point2D, Point2D> NodeTopLeftChanged;
        event Action<IDiagramConnector, Route, Route> ConnectorRouteChanged;
    }
}
