using System;
using System.Collections.Generic;

namespace Cradiator.Config
{
	public interface IConfigSettings 
	{
		string URL { get; set; }
		string SkinName { get; set; }
		int PollFrequency { get; set; }
		string ProjectNameRegEx { get; set; }
		bool ShowCountdown { get; set; }
		TimeSpan PollFrequencyTimeSpan { get; }
		string BrokenBuildSound { get; set; }
		string FixedBuildSound { get; set; }
		string BrokenBuildText { get; set; }
		string FixedBuildText { get; set; }
		bool PlaySounds { get; set; }
		bool PlaySpeech { get; set; }
		string SpeechVoiceName { get; set; }
		string CategoryRegEx { get; set; }
		bool ShowProgress { get; set; }
		IDictionary<string, string> UsernameMap { get; }
		GuiltStrategyType BreakerGuiltStrategy { get; }
		void AddObserver(IConfigObserver observer);
		void Load();
		void NotifyObservers();
		void Save();
	}
}