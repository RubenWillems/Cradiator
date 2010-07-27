using Cradiator.Config;
using Cradiator.Extensions;

namespace Cradiator.Model
{
	public interface IBuildBuster 
	{
		string FindBreaker(string currentMessage);
	}

	public class BuildBuster : IBuildBuster, IConfigObserver
	{
		BuildBusterStrategy _guiltStrategy;
		readonly FixerStrategy _fixerStrategy;
		readonly GuiltFactory _guiltFactory;

		public BuildBuster(IConfigSettings configSettings, FixerStrategy fixerStrategy, GuiltFactory guiltFactory)
		{
			_fixerStrategy = fixerStrategy;
			_guiltFactory = guiltFactory;
			SetGuiltStrategy(configSettings);
			configSettings.AddObserver(this);
		}

		void SetGuiltStrategy(IConfigSettings configSettings)
		{
			_guiltStrategy = _guiltFactory.Get(configSettings.BreakerGuiltStrategy);
		}

		public string FindBreaker(string currentMessage)
		{
			if (currentMessage.IsEmpty())
				return string.Empty;

			var username = string.Empty;

			if (_fixerStrategy.IsMatch(currentMessage))
				username = _fixerStrategy.Extract(currentMessage);

			if (_guiltStrategy.IsMatch(currentMessage))
				username = _guiltStrategy.Extract(currentMessage);

			return username;
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			SetGuiltStrategy(newSettings);
		}
	}
}