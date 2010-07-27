using System.Collections.Generic;
using Cradiator.Config;

namespace Cradiator.Model
{
	public class BuildBusterFullNameDecorator : IBuildBuster, IConfigObserver
	{
		readonly IBuildBuster _buildBuster;
		IDictionary<string, string> _usernameMap;

		public BuildBusterFullNameDecorator([InjectBuildBuster] IBuildBuster buildBuster, IConfigSettings configSettings)
		{
			_buildBuster = buildBuster;
			_usernameMap = configSettings.UsernameMap;
			configSettings.AddObserver(this);
		}

		public string FindBreaker(string sentence)
		{
			var username = _buildBuster.FindBreaker(sentence);

			if (_usernameMap.ContainsKey(username))
				username = _usernameMap[username];

			return username;
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_usernameMap = newSettings.UsernameMap;
		}
	}
}