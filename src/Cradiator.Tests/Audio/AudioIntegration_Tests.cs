using System.Speech.Synthesis;
using Cradiator.Audio;
using Cradiator.Config;
using NUnit.Framework;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	[Category("Integration")]
	[Ignore]
	public class AudioIntegration_Tests
	{
		[Test]
		public void CanPlay_WAV_File()
		{
			new AudioPlayer(@"c:\windows\media").Play("ding.wav");
		}

		[Test]
		public void SpeechSynthesisTest()
		{
			var builder = new PromptBuilder();
			builder.AppendText("The build breaker is, dave.");
			var player = new AudioPlayer(new CradiatorSpeechSynthesizer(new SpeechSynthesizer()), new ConfigSettings(),
			                             new VoiceSelector(null), new AppLocation());
			player.Say(builder);
		}
	}
}