using System;
using System.Collections.Generic;
using System.Linq;
using System.Speech.Synthesis;
using log4net;

namespace Cradiator.Audio
{
	public interface ISpeechSynthesizer
	{
		void SpeakAsync(PromptBuilder promptBuilder);
		void SelectVoice(string voiceName);
		IEnumerable<CradiatorInstalledVoice> GetInstalledVoices();
		int Rate { set; }
		CradiatorInstalledVoice SelectedVoice { get; }
	}

	/// <summary>
	/// an adapter to abstract speech synthesis away from the unfriendly SpeechSynthesizer class
	/// </summary>
	public class CradiatorSpeechSynthesizer : ISpeechSynthesizer
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(CradiatorSpeechSynthesizer).Name);

		readonly SpeechSynthesizer _speechSynth;

		public CradiatorSpeechSynthesizer(SpeechSynthesizer speechSynth)
		{
			_speechSynth = speechSynth;
		}

		public void SpeakAsync(PromptBuilder promptBuilder)
		{
			_speechSynth.SpeakAsync(promptBuilder);
		}

		public CradiatorInstalledVoice SelectedVoice 
		{
			get { return new CradiatorInstalledVoice(_speechSynth.Voice.Name); }
		}

		public void SelectVoice(string voiceName)
		{
			try
			{
				_speechSynth.SelectVoice(voiceName);
			}
			catch (Exception exception)
			{
				_log.Error(exception.Message, exception);	
				// can happen, even if a voice is installed and enabled (blame .NET SAPI?)
				// we don't want this to stop anything else from happening
			}
		}

		public IEnumerable<CradiatorInstalledVoice> GetInstalledVoices()
		{
			return from voice in _speechSynth.GetInstalledVoices()
			       select new CradiatorInstalledVoice(voice.VoiceInfo.Name);
		}

		public int Rate
		{
			set { _speechSynth.Rate = value; }
		}
	}
}