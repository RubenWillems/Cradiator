using Cradiator.Config;
using Cradiator.Services;
using Cradiator.Views;
using Ninject;

namespace Cradiator.App
{
	public class Bootstrapper
	{
		readonly IConfigSettings _configSettings;
		readonly ICradiatorView _view;

		public Bootstrapper(IConfigSettings configSettings, ICradiatorView view)
		{
			_configSettings = configSettings;
			_view = view;
		}

		public IKernel CreateKernel()
		{
		    var cruiseNinjaModule = new CradiatorNinjaModule(_view, _configSettings);
			return Ninjector.Kernel = new StandardKernel(cruiseNinjaModule);
		}
	}
}