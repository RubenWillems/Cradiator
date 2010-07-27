namespace Cradiator.Config
{
	public interface IConfigObserver
	{
		void ConfigUpdated(ConfigSettings newSettings);
	}
}