using System.IO;
using System.Reflection;

namespace Cradiator.Config
{
	public interface IConfigLocation 
	{
		string FileName { get; }
	}

	public class ConfigLocation : IConfigLocation
	{
		public string FileName
		{
			get { return Assembly.GetExecutingAssembly().Location + ".config"; }
		}
	}

	public interface IAppLocation
	{
		string DirectoryName { get; }
	}

	public class AppLocation : IAppLocation
	{
		public string DirectoryName
		{
			get { return Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location); }
		}
	}
}