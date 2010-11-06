using System;
using System.Collections.Generic;

namespace Cradiator.Config
{
	// ReSharper disable UnusedMemberInSuper.Global
	public interface IConfigSettings : IViewSettings
	{
		int PollFrequency { get; set; }
		bool ShowCountdown { get; set; }
		TimeSpan PollFrequencyTimeSpan { get; }
		string BrokenBuildSound { get; set; }
		string FixedBuildSound { get; set; }
		string BrokenBuildText { get; set; }
		string FixedBuildText { get; set; }
		bool PlaySounds { get; set; }
		bool PlaySpeech { get; set; }
		string SpeechVoiceName { get; set; }
		bool ShowProgress { get; set; }
		IDictionary<string, string> UsernameMap { get; }
		GuiltStrategyType BreakerGuiltStrategy { get; }
		void AddObserver(IConfigObserver observer);
		void Load();
		void NotifyObservers();
		void Save();

	    int ViewCount { get; }
		void UpdateViewSettings(int index);
	}
	// ReSharper restore UnusedMemberInSuper.Global
}
