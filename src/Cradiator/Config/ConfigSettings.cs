using System;
using System.Collections.Generic;
using System.Configuration;
using System.Reflection;
using Cradiator.Extensions;
using log4net;
using NotifyPropertyWeaver;

namespace Cradiator.Config
{
	/// <summary>
	/// configuration settings
	/// </summary>
    public class ConfigSettings : NotifyBase, IConfigSettings
	{
		const int DefaultPollingFrequency = 30;

		const string PollFrequencyKey = "PollFrequency";
		const string UrlKey = "URL";
		const string SkinKey = "Skin";
		const string ProjectNameRegexKey = "ProjectNameRegEx";
		const string CategoryRegexKey = "CategoryRegEx";
		const string ShowCountdownKey = "ShowCountdown";
		const string ShowProgressKey = "ShowProgress";
		const string PlaySoundsKey = "PlaySounds";
		const string BrokenBuildSoundKey = "BrokenBuildSound";
		const string FixedBuildSoundKey = "FixedBuildSound";
		const string BrokenBuildTextKey = "BrokenBuildText";
		const string FixedBuildTextKey = "FixedBuildText";
		const string PlaySpeechKey = "PlaySpeech";
		const string SpeechVoiceNameKey = "SpeechVoiceName";
		const string BreakerGuiltStrategyKey = "BreakerGuiltStrategy";

		readonly IList<IConfigObserver> _configObservers = new List<IConfigObserver>();
		static readonly ILog _log = LogManager.GetLogger(typeof(ConfigSettings).Name);
		IDictionary<string, string> _usernameMap = new Dictionary<string, string>();
		readonly UserNameMappingReader _userNameMappingReader = new UserNameMappingReader(new ConfigLocation());

		private string _projectNameRegEx;
		public string ProjectNameRegEx
		{
			get { return _projectNameRegEx.GetRegEx(); }
			set
			{
				if (_projectNameRegEx == value) return;
				_projectNameRegEx = value;
				OnPropertyChanged("ProjectNameRegEx");
			}
		}

		private string _categoryRegEx;
		public string CategoryRegEx
		{
			get { return _categoryRegEx.GetRegEx(); }
			set
			{
				if (_categoryRegEx == value) return;
				_categoryRegEx = value;
                OnPropertyChanged("CategoryRegEx");
			}
		}

        [NotifyProperty] public string URL { get; set; }
        [NotifyProperty] public string SkinName { get; set; } 
        [NotifyProperty] public int PollFrequency { get; set; }
        [NotifyProperty] public bool ShowCountdown { get; set; }
        [NotifyProperty] public bool ShowProgress { get; set; }
        [NotifyProperty] public string BrokenBuildSound { get; set; }
        [NotifyProperty] public string FixedBuildSound { get; set; }
        [NotifyProperty] public string BrokenBuildText { get; set; }
        [NotifyProperty] public string FixedBuildText { get; set; }
        [NotifyProperty] public bool PlaySounds { get; set; }
        [NotifyProperty] public bool PlaySpeech { get; set; }
        [NotifyProperty] public string SpeechVoiceName { get; set; }

        public IDictionary<string, string> UsernameMap
        {
            get { return _usernameMap; }
            set { _usernameMap = value; }
        }

		private string _breakerGuiltStrategy;
		public GuiltStrategyType BreakerGuiltStrategy
		{
			get { return _breakerGuiltStrategy == "First" ? GuiltStrategyType.First : GuiltStrategyType.Last; }
		}

		public TimeSpan PollFrequencyTimeSpan
		{
			get { return TimeSpan.FromSeconds(PollFrequency); }
		}

		public void Save()
		{
			try
			{
				var config = OpenExeConfiguration();

				config.AppSettings.Settings[UrlKey].Value = URL;
				config.AppSettings.Settings[SkinKey].Value = SkinName;
				config.AppSettings.Settings[PollFrequencyKey].Value = PollFrequency.ToString();
				config.AppSettings.Settings[ProjectNameRegexKey].Value = ProjectNameRegEx;
				config.AppSettings.Settings[CategoryRegexKey].Value = CategoryRegEx;
				config.AppSettings.Settings[ShowCountdownKey].Value = ShowCountdown.ToString();
				config.AppSettings.Settings[ShowProgressKey].Value = ShowProgress.ToString();
                config.AppSettings.Settings[PlaySoundsKey].Value = PlaySounds.ToString();
                config.AppSettings.Settings[PlaySpeechKey].Value = PlaySpeech.ToString();
				config.AppSettings.Settings[BrokenBuildSoundKey].Value = BrokenBuildSound;
				config.AppSettings.Settings[FixedBuildSoundKey].Value = FixedBuildSound;
				config.AppSettings.Settings[BrokenBuildTextKey].Value = BrokenBuildText;
				config.AppSettings.Settings[FixedBuildTextKey].Value = FixedBuildText;
				config.AppSettings.Settings[SpeechVoiceNameKey].Value = SpeechVoiceName;
				config.AppSettings.Settings[BreakerGuiltStrategyKey].Value = _breakerGuiltStrategy;
				config.Save(ConfigurationSaveMode.Minimal);
			}
			catch (Exception ex)
			{
				// config may be edited in the file (manually) - we cannot show an error dialog here
				// because it's entirely reasonable that the user doesn't have access to the machine running the exe to close it
				_log.Error(ex.Message, ex);		
			}
		}

	    public void Load()
	    {
	        var config = OpenExeConfiguration();

            URL = config.GetProperty(UrlKey).Value;
			SkinName = config.GetProperty(SkinKey).Value;
            PollFrequency = config.GetIntProperty(PollFrequencyKey, DefaultPollingFrequency);
			ProjectNameRegEx = config.GetProperty(ProjectNameRegexKey).Value;
			CategoryRegEx = config.GetProperty(CategoryRegexKey).Value;
            ShowCountdown = config.GetBoolProperty(ShowCountdownKey);
            ShowProgress = config.GetBoolProperty(ShowProgressKey);
			PlaySounds = config.GetBoolProperty(PlaySoundsKey);
			PlaySpeech = config.GetBoolProperty(PlaySpeechKey);
			BrokenBuildSound = config.GetProperty(BrokenBuildSoundKey).Value;
			FixedBuildSound = config.GetProperty(FixedBuildSoundKey).Value;
			BrokenBuildText = config.GetProperty(BrokenBuildTextKey).Value;
			FixedBuildText = config.GetProperty(FixedBuildTextKey).Value;
			SpeechVoiceName = config.GetProperty(SpeechVoiceNameKey).Value;
			_breakerGuiltStrategy = config.GetProperty(BreakerGuiltStrategyKey).Value;

	    	_usernameMap = _userNameMappingReader.Read();
	    }

		private static Configuration OpenExeConfiguration()
		{
			return ConfigurationManager.OpenExeConfiguration(Assembly.GetExecutingAssembly().Location);
		}

		public void AddObserver(IConfigObserver observer)
		{
			_configObservers.Add(observer);
		}

		public void NotifyObservers()
		{
			foreach (var observer in _configObservers)
			{
				observer.ConfigUpdated(this);
			}
		}

		public override string ToString()
		{
			return string.Format("Url={0}, SkinName={1}, PollFrequency={2}, ProjectNameRegEx={3}, ShowCountdown={4}, ShowCountdown={5}, PlaySounds={6}, PlaySpeech={7}, BrokenBuildSound={8}, BrokenBuildText={9}, FixedBuildSound={10}, FixedBuildText={11}, SpeechVoiceName={12}, CategoryRegEx={13}, BreakerGuiltStrategy={14}",
                                 URL, SkinName, PollFrequency, ProjectNameRegEx, ShowCountdown, ShowProgress, PlaySounds, PlaySpeech, BrokenBuildSound, BrokenBuildText, FixedBuildSound, FixedBuildText, SpeechVoiceName, CategoryRegEx, BreakerGuiltStrategy);
		}
	}

	public enum GuiltStrategyType
	{
		First,
		Last
	}

	public class ConfigSettingsException : Exception
    {
        public ConfigSettingsException(string key) : 
			base(string.Format("Configuration setting missing: '{0}'", key)) 
		{  }
    }
}