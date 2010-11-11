using System.Linq;
using Cradiator.Extensions;

namespace Cradiator.Audio
{
	public class VoiceSelector
	{
		readonly ISpeechSynthesizer _speechSynth;

		public VoiceSelector(ISpeechSynthesizer speechSynth)
		{
			_speechSynth = speechSynth;
		}

		public void SelectInstalledVoice(string voiceName)
		{
			var matchingVoiceName = GetClosestMatchingInstalledVoice(voiceName).Name;
			_speechSynth.SelectVoice(matchingVoiceName);
		}

		public CradiatorInstalledVoice GetClosestMatchingInstalledVoice(string voiceName)
		{
			if (voiceName.IsEmpty()) return _speechSynth.SelectedVoice;

			var selectedVoices =
				from voice in _speechSynth.GetInstalledVoices()
				where voice.Name.ContainsIgnoreCase(voiceName)
				orderby voice.Name
				select voice;

			return selectedVoices.Any() ? selectedVoices.First() : _speechSynth.SelectedVoice;
		}
	}
}