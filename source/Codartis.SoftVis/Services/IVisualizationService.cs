﻿using Codartis.SoftVis.Diagramming;
using Codartis.SoftVis.Modeling.Definition;
using Codartis.SoftVis.UI;

namespace Codartis.SoftVis.Services
{
    /// <summary>
    /// Provides model, diagram and UI services.
    /// </summary>
    public interface IVisualizationService
    {
        DiagramId CreateDiagram(double minZoom, double maxZoom, double initialZoom);

        IModelService GetModelService();
        IDiagramService GetDiagramService(DiagramId diagramId);
        IUiService GetUiService(DiagramId diagramId);
    }
}
