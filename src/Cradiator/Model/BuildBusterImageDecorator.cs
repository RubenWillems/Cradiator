using System.IO;
using Cradiator.Config;
using log4net;

namespace Cradiator.Model
{
	public class BuildBusterImageDecorator : IBuildBuster
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(BuildBusterImageDecorator).Name);

		readonly IBuildBuster _buildBuster;
		readonly string _imageFolder;

		public BuildBusterImageDecorator([InjectBuildBuster] IBuildBuster buildBuster, IAppLocation appLocation)
		{
			_buildBuster = buildBuster;
			_imageFolder = Path.Combine(appLocation.DirectoryName, "images");
		}

		public string FindBreaker(string currentMessage)
		{
			var username = _buildBuster.FindBreaker(currentMessage);
			var imagePath = string.Format(@"{0}\{1}.jpg", _imageFolder, username).Trim();

			_log.DebugFormat("Breaker image='{0}'", imagePath);

			return imagePath;
		}
	}
}