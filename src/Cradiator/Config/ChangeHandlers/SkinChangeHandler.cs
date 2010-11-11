using Cradiator.Model;

namespace Cradiator.Config.ChangeHandlers
{
	public class SkinChangeHandler : IConfigChangeHandler
	{
		readonly ISkinLoader _skinLoader;
		Skin _currentSkin;

		public SkinChangeHandler(ISkinLoader skinLoader)
		{
			_skinLoader = skinLoader;
		}

		void IConfigChangeHandler.ConfigUpdated(ConfigSettings newSettings)
		{
			if (_currentSkin == null || _currentSkin.Name != newSettings.SkinName)
			{
				var newSkin = new Skin(newSettings.SkinName);

				_skinLoader.Load(newSkin);

				_currentSkin = newSkin;
			}
		}
	}
}