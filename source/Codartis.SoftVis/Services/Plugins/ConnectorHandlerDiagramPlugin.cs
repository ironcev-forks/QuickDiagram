﻿using Codartis.SoftVis.Diagramming;
using Codartis.SoftVis.Diagramming.Events;
using Codartis.SoftVis.Modeling.Definition;

namespace Codartis.SoftVis.Services.Plugins
{
    /// <summary>
    /// Automatically shows relationships when both ends are visible
    /// and removes redundant connectors from the diagram.
    /// </summary>
    public class ConnectorHandlerDiagramPlugin : ConnectorManipulatorDiagramPluginBase
    {
        public override void Initialize(IModelService modelService, IDiagramService diagramService)
        {
            base.Initialize(modelService, diagramService);

            DiagramService.DiagramChanged += OnDiagramChanged;
        }

        public override void Dispose()
        {
            DiagramService.DiagramChanged -= OnDiagramChanged;
        }

        private void OnDiagramChanged(DiagramEventBase diagramEvent)
        {
            var model = ModelService.LatestModel;
            var diagram = diagramEvent.NewDiagram;

            switch (diagramEvent)
            {
                case DiagramNodeAddedEvent diagramNodeAddedEvent:
                    var modelNode = diagramNodeAddedEvent.NewNode.ModelNode;
                    ShowModelRelationshipsIfBothEndsAreVisible(modelNode, model, diagram);
                    break;

                case DiagramNodeRemovedEvent _:
                    // TODO: optimize: should check only surrounding nodes of the deleted one (not all in diagram)
                    foreach (var diagramNode in diagram.Nodes)
                        ShowModelRelationshipsIfBothEndsAreVisible(diagramNode.ModelNode, model, diagram);
                    break;

                case DiagramConnectorAddedEvent _:
                    HideRedundantConnectors(diagram);
                    break;

                    // DiagramConnectorRemovedEvent is not handled 
                    // because that would put back removed connectors immediately
                    // (because nodes are removed after connectors)
            }
        }

        private void HideRedundantConnectors(IDiagram diagram)
        {
            foreach (var connector in diagram.Connectors)
                if (diagram.IsConnectorRedundant(connector.Id))
                    DiagramService.RemoveConnector(connector.Id);
        }
    }
}
