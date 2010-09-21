using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cradiator.Config;
using Cradiator.Extensions;
using log4net;

namespace Cradiator.Model
{
	public class BuildBusterImageDecorator : IBuildBuster
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(BuildBusterImageDecorator).Name);

		readonly IBuildBuster _buildBuster;
		readonly string _imageFolder;
		readonly List<char> InvalidCharacterList = Path.GetInvalidFileNameChars().ToList();

		public BuildBusterImageDecorator([InjectBuildBuster] IBuildBuster buildBuster, IAppLocation appLocation)
		{
			_buildBuster = buildBuster;
			_imageFolder = Path.Combine(appLocation.DirectoryName, "images");
		}

		public string FindBreaker(string currentMessage)
		{
			var username = _buildBuster.FindBreaker(currentMessage);

			if (username.ContainsInvalidChars())
			{
				foreach (var c in username.ToCharArray().Where(InvalidCharacterList.Contains))
				{
					username = username.Replace(c.ToString(), "");
				}
			}

			var imagePath = string.Format(@"{0}\{1}.jpg", _imageFolder, username).Trim();

			_log.DebugFormat("Breaker image='{0}'", imagePath);

			return imagePath;
		}
	}
}