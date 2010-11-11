using System;
using System.IO;
using System.Media;
using System.Speech.Synthesis;
using Cradiator.Config;
using log4net;
using Ninject;

namespace Cradiator.Audio
{
	public interface IAudioPlayer
	{
		void Play(string filename);
		void Say(PromptBuilder promptBuilder);
	}

	public class AudioPlayer : IAudioPlayer, IConfigObserver
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(AudioPlayer).Name);

		readonly ISpeechSynthesizer _speechSynth;
		readonly VoiceSelector _voiceSelector;
		readonly string _wavFileFolder;
		SoundPlayer _soundPlayer;

		bool _playSounds;
		bool _playSpeech;
	    string _speechVoiceName;

		[Inject]
		public AudioPlayer(ISpeechSynthesizer speechSynth, IConfigSettings configSettings, 
						   VoiceSelector voiceSelector, IAppLocation appLocation)
		{
			_voiceSelector = voiceSelector;
			_speechVoiceName = configSettings.SpeechVoiceName;
			_playSounds = configSettings.PlaySounds;
			_playSpeech = configSettings.PlaySpeech;
            _wavFileFolder = Path.Combine(appLocation.DirectoryName, "sounds");

			_speechSynth = speechSynth;
			_speechSynth.Rate = -2;		//TODO might be useful as configuration

			configSettings.AddObserver(this);
		}

		/// <summary>
		/// this is for testing only, TODO reconsider
		/// </summary>
		public AudioPlayer(string path)
		{
			_wavFileFolder = path;
			_playSounds = _playSpeech = true;
		}

		public void Say(PromptBuilder promptBuilder)
		{
			if (!_playSpeech) return;

			try
			{
                _voiceSelector.SelectInstalledVoice(_speechVoiceName);
				_speechSynth.SpeakAsync(promptBuilder);
			}
			catch (Exception exception)
			{
				_log.Error("SpeechSynthesizer failed", exception);
			}
		}

		public void Play(string soundFileName)
		{
			if (!_playSounds) return;

			var soundFile = "";
			try
			{
				soundFile = Path.Combine(_wavFileFolder, soundFileName);

				_soundPlayer = new SoundPlayer(soundFile);
				_soundPlayer.Play();
			}
			catch (Exception exception)
			{
				_log.ErrorFormat("Failed to play sound file {0} {1}", soundFile, exception);
			}
		}

		void IConfigObserver.ConfigUpdated(ConfigSettings newSettings)
		{
			_playSounds = newSettings.PlaySounds;
			_playSpeech = newSettings.PlaySpeech;
		    _speechVoiceName = newSettings.SpeechVoiceName;
		}
	}
}