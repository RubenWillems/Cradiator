using System.Collections.Generic;
using log4net;
using Ninject;

namespace Cradiator.Config.ChangeHandlers
{
	/// <summary>
	/// The Farm takes care of ChangeHandlers - who do not subscribe to config change events themselves
	/// They are animals, who require herding... infact, I'm wondering
	/// whether to change this class to ConfigChangeSheepDog
	/// </summary>
	public class ConfigChangeHandlerFarm
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(ConfigChangeHandlerFarm).Name);

		readonly List<IConfigChangeHandler> _changeHandlers = new List<IConfigChangeHandler>();

		[Inject]
		public ConfigChangeHandlerFarm(IEnumerable<IConfigChangeHandler> observers)
		{
			_changeHandlers.AddRange(observers);
		}

		public void UpdateAll(ConfigSettings newSettings)
		{
			_log.InfoFormat("New settings: {0}", newSettings.ToString());

			foreach (var handler in _changeHandlers)
			{
				handler.ConfigUpdated(newSettings);
			}
		}
	}
}