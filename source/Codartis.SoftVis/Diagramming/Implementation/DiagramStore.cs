﻿using System;
using System.Diagnostics;
using Codartis.SoftVis.Diagramming.Events;
using Codartis.SoftVis.Geometry;
using Codartis.SoftVis.Modeling.Definition;
using Codartis.Util;
using JetBrains.Annotations;

namespace Codartis.SoftVis.Diagramming.Implementation
{
    /// <summary>
    /// Implements a diagram mutator.
    /// Keeps a current diagram version, implements mutator operations and publishes change events.
    /// The underlying model is immutable so each modification creates a new snapshot of the diagram.
    /// </summary>
    /// <remarks>
    /// Mutators must not run concurrently. A lock ensures it.
    /// </remarks>
    public sealed class DiagramStore : IDiagramStore
    {
        public IDiagram Diagram { get; private set; }

        private readonly object _diagramUpdateLockObject = new object();

        public event Action<DiagramEventBase> DiagramChanged;

        public DiagramStore(IDiagram diagram)
        {
            Diagram = diagram ?? throw new ArgumentNullException(nameof(diagram));
        }

        public void AddNode(IDiagramNode node)
        {
            lock (_diagramUpdateLockObject)
            {
                if (Diagram.NodeExists(node.Id))
                    return;

                Diagram = Diagram.AddNode(node);
                DiagramChanged?.Invoke(new DiagramNodeAddedEvent(Diagram, node));
            }
        }

        public void RemoveNode(ModelNodeId nodeId)
        {
            lock (_diagramUpdateLockObject)
            {
                if (!Diagram.NodeExists(nodeId))
                    return;

                var oldNode = Diagram.GetNode(nodeId);
                Diagram = Diagram.RemoveNode(nodeId);
                DiagramChanged?.Invoke(new DiagramNodeRemovedEvent(Diagram, oldNode));
            }
        }

        public void UpdateDiagramNodeModelNode(IDiagramNode diagramNode, IModelNode newModelNode)
        {
            lock (_diagramUpdateLockObject)
            {
                Diagram
                    .TryGetNode(diagramNode.Id)
                    .Match(
                        oldNode =>
                        {
                            var updatedNode = oldNode.WithModelNode(newModelNode);
                            Diagram = Diagram.UpdateNode(updatedNode);
                            DiagramChanged?.Invoke(new DiagramNodeModelNodeChangedEvent(Diagram, oldNode, updatedNode));
                        },
                        () => throw new InvalidOperationException($"Node {diagramNode} does not exist."));
            }
        }

        public void UpdateDiagramNodeSize(IDiagramNode diagramNode, Size2D newSize)
        {
            lock (_diagramUpdateLockObject)
            {
                Diagram
                    .TryGetNode(diagramNode.Id)
                    .Match(
                        oldNode =>
                        {
                            var updatedNode = oldNode.WithSize(newSize);
                            Diagram = Diagram.UpdateNode(updatedNode);
                            DiagramChanged?.Invoke(new DiagramNodeSizeChangedEvent(Diagram, oldNode, updatedNode));
                        },
                        () => throw new InvalidOperationException($"Node {diagramNode} does not exist."));
            }
        }

        public void UpdateDiagramNodeCenter(IDiagramNode diagramNode, Point2D newCenter)
        {
            lock (_diagramUpdateLockObject)
            {
                Diagram
                    .TryGetNode(diagramNode.Id)
                    .Match(
                        oldNode =>
                        {
                            var updatedNode = oldNode.WithCenter(newCenter);
                            Diagram = Diagram.UpdateNode(updatedNode);
                            DiagramChanged?.Invoke(new DiagramNodePositionChangedEvent(Diagram, oldNode, updatedNode));
                        },
                        () => Debug.WriteLine($"Trying to move node {diagramNode} but it does not exist."));
            }
        }

        public void UpdateDiagramNodeTopLeft(IDiagramNode diagramNode, Point2D newTopLeft)
        {
            lock (_diagramUpdateLockObject)
            {
                Diagram
                    .TryGetNode(diagramNode.Id)
                    .Match(
                        oldNode =>
                        {
                            var updatedNode = oldNode.WithTopLeft(newTopLeft);
                            Diagram = Diagram.UpdateNode(updatedNode);
                            DiagramChanged?.Invoke(new DiagramNodePositionChangedEvent(Diagram, oldNode, updatedNode));
                        },
                        () => throw new InvalidOperationException($"Node {diagramNode} does not exist."));
            }
        }

        public void AddConnector([NotNull] IDiagramConnector connector)
        {
            lock (_diagramUpdateLockObject)
            {
                if (Diagram.ConnectorExists(connector.Id))
                    return;

                Diagram = Diagram.AddConnector(connector);
                DiagramChanged?.Invoke(new DiagramConnectorAddedEvent(Diagram, connector));
            }
        }

        public void RemoveConnector(ModelRelationshipId connectorId)
        {
            lock (_diagramUpdateLockObject)
            {
                if (!Diagram.ConnectorExists(connectorId))
                    return;

                var oldConnector = Diagram.GetConnector(connectorId);
                Diagram = Diagram.RemoveConnector(connectorId);
                DiagramChanged?.Invoke(new DiagramConnectorRemovedEvent(Diagram, oldConnector));
            }
        }

        public void UpdateConnectorRoute(ModelRelationshipId connectorId, Route newRoute)
        {
            lock (_diagramUpdateLockObject)
            {
                Diagram
                    .TryGetConnector(connectorId)
                    .Match(
                        oldConnector =>
                        {
                            var newConnector = oldConnector.WithRoute(newRoute);
                            Diagram = Diagram.UpdateConnector(newConnector);
                            DiagramChanged?.Invoke(new DiagramConnectorRouteChangedEvent(Diagram, oldConnector, newConnector));
                        },
                        () => throw new InvalidOperationException($"Connector {connectorId} does not exist."));
            }
        }

        public void ClearDiagram()
        {
            lock (_diagramUpdateLockObject)
            {
                Diagram = Diagram.Clear();
                DiagramChanged?.Invoke(new DiagramClearedEvent(Diagram));
            }
        }
    }
}