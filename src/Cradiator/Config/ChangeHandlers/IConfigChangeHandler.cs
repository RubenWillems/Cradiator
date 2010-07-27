namespace Cradiator.Config.ChangeHandlers
{
	public interface IConfigChangeHandler
	{
		void ConfigUpdated(ConfigSettings newSettings);
	}
}