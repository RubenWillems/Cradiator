using System.Collections.Generic;
using Cradiator.Audio;
using Cradiator.Config;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class SpeechMaker_Tests
	{
		const string ProjectFixedText = "is fixed";
		const string ProjectBrokenText = "is broken";

		List<ProjectStatus> _projectStatusList;
		SpeechMaker _speaker;
		ISpeechTextParser _speechTextParser;

		[SetUp]
		public void SetUp()
		{
			MockRepository.GenerateStub<IBuildBuster>();
			_speechTextParser = MockRepository.GenerateMock<ISpeechTextParser>();

			_projectStatusList = new List<ProjectStatus>
			                     {
			                     	new ProjectStatus("Project2")
			                     	{
			                     		LastBuildStatus = ProjectStatus.SUCCESS
			                     	}
			                     };

			_speaker = new SpeechMaker(new ConfigSettings
			                           {
											FixedBuildText = ProjectFixedText,
			                           		BrokenBuildText = ProjectBrokenText
			                           }, _speechTextParser);
		}

		[Test]
		public void CanMakeSpeech_ForBuildIsFixed()
		{
			_speechTextParser.Expect(s => s.Parse(null, null)).IgnoreArguments().Return(ProjectFixedText);

			var speech = _speaker.BuildIsFixed(_projectStatusList);
			Assert.That(speech.ToXml(), Text.Contains(ProjectFixedText));
		}

		[Test]
		public void CanMakeSpeech_ForBuildIsBroken()
		{
			_speechTextParser.Expect(s => s.Parse(null, null)).IgnoreArguments().Return(ProjectBrokenText);

			var speech = _speaker.BuildIsBroken(_projectStatusList);
			Assert.That(speech.ToXml(), Text.Contains(ProjectBrokenText));
		}

		[Test]
		public void CanUpdateSettings()
		{
			_speechTextParser.Expect(s => s.Parse(Arg.Is(ProjectFixedText), Arg<ProjectStatus>.Is.Anything))
				.Return("dont care");

			_speaker.BuildIsFixed(_projectStatusList);

			_speechTextParser.Expect(s => s.Parse(Arg.Is("is different"), Arg<ProjectStatus>.Is.Anything))
				.Return("dont care");

			_speaker.ConfigUpdated(new ConfigSettings { FixedBuildText = "is different" });

			_speaker.BuildIsFixed(_projectStatusList);

			_speechTextParser.VerifyAllExpectations();
		}
	}
}