using System.Collections.Generic;
using Ninject;

namespace Cradiator.Config.ChangeHandlers
{
	/// <summary>
	/// The Farm takes care of ChangeHandlers - who do not subscribe to config change events themselves
	/// They are animals, who require herding... could also be called ConfigChangeSheepDog
	/// </summary>
	public class ConfigChangeHandlerFarm
	{
		readonly List<IConfigChangeHandler> _changeHandlers = new List<IConfigChangeHandler>();

		[Inject]
		public ConfigChangeHandlerFarm(IEnumerable<IConfigChangeHandler> observers)
		{
			_changeHandlers.AddRange(observers);
		}

		public void UpdateAll(ConfigSettings newSettings)
		{
			foreach (var handler in _changeHandlers)
			{
				handler.ConfigUpdated(newSettings);
			}
		}
	}
}