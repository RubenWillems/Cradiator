using System.Collections.Generic;
using Cradiator.Audio;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class VoiceSelector_Tests
	{
		const string CepstralWilliam = "Cepstral William";

		ISpeechSynthesizer _speechSynth;
		VoiceSelector _voiceSelector;

		[SetUp]
		public void SetUp()
		{
			_speechSynth = MockRepository.GenerateMock<ISpeechSynthesizer>();
			_voiceSelector = new VoiceSelector(_speechSynth);
		}

		[Test]
		public void CanNotSet_SpeechVoiceName_IfNameNotIn_InstalledVoices()
		{
			_speechSynth.Expect(s => s.SelectedVoice).Return(new CradiatorInstalledVoice("Bob"));
			_speechSynth.Expect(s => s.GetInstalledVoices())
				.Return(new List<CradiatorInstalledVoice>
				        {
				        	new CradiatorInstalledVoice("Bob"),
				        });

			_voiceSelector.SelectInstalledVoice("DodgyVoiceName");
			_speechSynth.AssertWasCalled(s => s.SelectVoice(Arg.Is("Bob")));
		}

		[Test]
		public void CanSet_SpeechVoiceName_From_2_InstalledVoices()
		{
			const string Bob = "Bob";
			_speechSynth.Expect(s => s.GetInstalledVoices())
				.Return(new List<CradiatorInstalledVoice>
				        {
				        	new CradiatorInstalledVoice(Bob),
				        	new CradiatorInstalledVoice("Mary")
				        });

			_voiceSelector.SelectInstalledVoice(Bob);
			_speechSynth.AssertWasCalled(s => s.SelectVoice(Arg.Is(Bob)));
		}

		[Test]
		public void CanSet_SpeechVoiceName_FromMany_InstalledVoices()
		{
			const string Terry = "Terry";
			_speechSynth.Expect(s => s.GetInstalledVoices())
				.Return(new List<CradiatorInstalledVoice>
				        {
				        	new CradiatorInstalledVoice("Bob"),
				        	new CradiatorInstalledVoice("Mary"),
				        	new CradiatorInstalledVoice("Mike"),
				        	new CradiatorInstalledVoice(Terry),
				        	new CradiatorInstalledVoice("Dexter")
				        });

			_voiceSelector.SelectInstalledVoice(Terry);
			_speechSynth.AssertWasCalled(s => s.SelectVoice(Arg.Is(Terry)));
		}

        /// <summary>
        /// we don't want to have to get the name exactly right ("william" is good enough to get "Cepstral William")
        /// if it's ever in doubt, then it's up to the user to specify the voice name exactly - or get the 'first one' that matches
        /// </summary>
        [Test]
        public void CanSet_SpeechVoiceName_IfTextContainsVoiceName_IeIsNotExactMatch()
        {
        	_speechSynth.Expect(s => s.GetInstalledVoices())
        		.Return(new List<CradiatorInstalledVoice>
        		        {
        		        	new CradiatorInstalledVoice(CepstralWilliam),
        		        });

			_voiceSelector.SelectInstalledVoice("william");
            _speechSynth.AssertWasCalled(s => s.SelectVoice(Arg.Is(CepstralWilliam)));
        }

		[Test]
		public void CanChoose_FirstOrderedVoiceName_If_MoreThan_1_Voice_Matches()
		{
			const string AppleWilliam = "Apple William ";

			_speechSynth.Expect(s => s.GetInstalledVoices())
				.Return(new List<CradiatorInstalledVoice>
				        {
				        	new CradiatorInstalledVoice("Microsoft William"),
				        	new CradiatorInstalledVoice(AppleWilliam),
				        	new CradiatorInstalledVoice(CepstralWilliam),
				        });

			_voiceSelector.SelectInstalledVoice("william");
			_speechSynth.AssertWasCalled(s => s.SelectVoice(Arg.Is(AppleWilliam)));
		}

		[Test]
		public void CanGetInstalledVoice_WithoutSelecting()
		{
			_speechSynth.Expect(s => s.SelectedVoice).Return(new CradiatorInstalledVoice("Bob"));

			_speechSynth.Expect(s => s.GetInstalledVoices())
				.Return(new List<CradiatorInstalledVoice>
				        {
				        	new CradiatorInstalledVoice("Bob"),
				        });

			Assert.That(_voiceSelector.GetClosestMatchingInstalledVoice("DodgyVoiceName"),
				Is.EqualTo(new CradiatorInstalledVoice("Bob")));
			_speechSynth.AssertWasNotCalled(s => s.SelectVoice(Arg<string>.Is.Anything));
		}

		[Test]
		public void CanReturnSelectedVoiceIfNullOrEmpty()
		{
			_speechSynth.Expect(s => s.SelectedVoice).Return(new CradiatorInstalledVoice("Bob"));
			Assert.That(_voiceSelector.GetClosestMatchingInstalledVoice(null).Name, Is.EqualTo("Bob"));
		}
	}
}