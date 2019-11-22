﻿using Codartis.SoftVis.Diagramming.Definition;
using Codartis.SoftVis.Modeling.Definition;

namespace Codartis.SoftVis.UI.Wpf.ViewModel
{
    /// <summary>
    /// A button for removing a shape from the diagram.
    /// </summary>
    public class CloseMiniButtonViewModel : MiniButtonViewModelBase
    {
        public CloseMiniButtonViewModel(
            IModelEventSource modelEventSource,
            IDiagramEventSource diagramEventSource)
            : base(modelEventSource, diagramEventSource, "Remove")
        {
            IsEnabled = true;
        }

        /// <summary>
        /// For one-off buttons the placement key is the type of the viewmodel.
        /// </summary>
        public override object PlacementKey => this.GetType();

        protected override void OnClick()
        {
            if (HostUiElement is DiagramNodeViewModel diagramNodeViewModel)
                diagramNodeViewModel.Remove();
        }
    }
}