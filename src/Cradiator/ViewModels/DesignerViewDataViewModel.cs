using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Ninject;
using Cradiator.Services;
using Cradiator.Model;

namespace Cradiator.ViewModels
{
    /// <summary>
    /// A 'viewModel' for CradiatorWindow.xaml
    /// just added this so we can use the designer again in visual studio, and see what effect
    /// a converter has for example, or brushes.
    /// Needed because of the ninject in the converters
    /// </summary>
    public class DesignerViewDataViewModel : ViewData
    {

        public DesignerViewDataViewModel()
        {
            this.Projects = new List<Model.ProjectStatus>();

            var cruiseDesignModule = new DesignTimeCradiatorNinjaModule();
            Ninjector.Kernel = new StandardKernel(cruiseDesignModule);

        }
    }
}
