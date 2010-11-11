using System.Collections.Generic;
using System.Speech.Synthesis;
using Cradiator.Audio;
using Cradiator.Config;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class AudioPlayer_Tests
	{
		ISpeechSynthesizer _speechSynth;
		IAppLocation _appLocation;

		[SetUp]
		public void SetUp()
		{
			_speechSynth = Create.Mock<ISpeechSynthesizer>();
			_appLocation = Create.Stub<IAppLocation>();
			_appLocation.Stub(a => a.DirectoryName).Return(@"c:\bla");
		}

		[Test]
		public void DoesNotSay_If_PlaySpeech_IsOff()
		{
			_speechSynth.Expect(s => s.GetInstalledVoices()).Return(new List<CradiatorInstalledVoice>());

			var audioPlayer = new AudioPlayer(_speechSynth,
											  new ConfigSettings { PlaySpeech = false },
											  new VoiceSelector(_speechSynth), _appLocation);
			audioPlayer.Say(new PromptBuilder());
			_speechSynth.AssertWasNotCalled(s=>s.SpeakAsync(Arg<PromptBuilder>.Is.Anything));
		}

		[Test]
		public void DoesSay_If_PlaySpeech_IsOn()
		{
			_speechSynth.Expect(s => s.GetInstalledVoices()).Return(new List<CradiatorInstalledVoice>
																	{
																		new CradiatorInstalledVoice("Bob")
																	});
			var audioPlayer = new AudioPlayer(_speechSynth,
											  new ConfigSettings { PlaySpeech = true, SpeechVoiceName = "Bob" },
											  new VoiceSelector(_speechSynth), _appLocation);
			audioPlayer.Say(new PromptBuilder());
			_speechSynth.ShouldHaveBeenCalled(s => s.SpeakAsync(Arg<PromptBuilder>.Is.Anything));
		}

		[Test]
		public void CanPlay_WAV_File()
		{
			//TODO make this test not so whimpy
			Assert.DoesNotThrow(()=>new AudioPlayer(@"c:\windows\media"));
		}
	}
}