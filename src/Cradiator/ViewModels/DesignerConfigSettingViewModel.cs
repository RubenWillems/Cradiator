using Cradiator.Config;
using Cradiator.Services;
using Ninject;
using System.ComponentModel;

namespace Cradiator.ViewModels
{
    /// <summary>
    /// A 'viewModel' for a settings class
    /// just added this so we can use the designer again in visual studio, and see what effect
    /// a converter has for example, or brushes.
    /// </summary>
    public class DesignerConfigSettingViewModel : ConfigSettings
    {

        public DesignerConfigSettingViewModel()
        {
            //to get the converters works and in so the designer
            var cruiseDesignModule = new DesignTimeCradiatorNinjaModule();
            Ninjector.Kernel = new StandardKernel(cruiseDesignModule);
        }
    }
}
