using System;
using System.IO;
using System.Threading;
using log4net;

namespace Cradiator.Config
{
	public interface IConfigFileWatcher 
	{
		void Start();
	}

	public class ConfigFileWatcher : IConfigFileWatcher
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(ConfigFileWatcher).Name);
		const long DelayInMilliseconds = 100;

		readonly string _configFile;
		readonly IConfigSettings _configSettings;
		FileSystemWatcher _fileSystemWatcher;
		Timer _timer;

		public FileSystemWatcher FileSystemWatcher
		{
			get { return _fileSystemWatcher; }
		}

		public ConfigFileWatcher(IConfigSettings configSettings, string configFile)
		{
			_configSettings = configSettings;
			_configFile = configFile;
		}

		public void Start()
		{
			_fileSystemWatcher = CreateFileSystemWatcher();
			_fileSystemWatcher.EnableRaisingEvents = true;
		}

		FileSystemWatcher CreateFileSystemWatcher() 
		{
			// Create the timer that will be used to deliver events. Set as disabled
			_timer = new Timer(OnDelayedForBatching_ConfigFileChanged, null, Timeout.Infinite, Timeout.Infinite);

			var watcher = new FileSystemWatcher
			{
				NotifyFilter = NotifyFilters.LastWrite,
			    Path = Path.GetDirectoryName(_configFile),
			    Filter = Path.GetFileName(_configFile)
			};

			watcher.Changed += OnConfigFileChanged;
			return watcher;
		}

		void OnConfigFileChanged(object sender, FileSystemEventArgs e)
		{
			_timer.Change(DelayInMilliseconds, Timeout.Infinite);
		}

		void OnDelayedForBatching_ConfigFileChanged(object state)
		{
			try
			{
				_configSettings.Load();
				_configSettings.NotifyObservers();
				_configSettings.Log();

			}
			catch (Exception exception)
			{
				_log.Error(exception);
			}	
		}
	}
}