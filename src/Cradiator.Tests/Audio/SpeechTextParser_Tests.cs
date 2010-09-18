using Cradiator.Audio;
using Cradiator.Model;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Audio
{
	[TestFixture]
	public class SpeechTextParser_Tests
	{
		IBuildBuster _buildBuster;
		SpeechTextParser _speechTextParser;
		ProjectStatus _failedProject;

		[SetUp]
		public void SetUp()
		{
			_buildBuster = Create.Stub<IBuildBuster>();
			_speechTextParser = new SpeechTextParser(_buildBuster);

			_failedProject = new ProjectStatus("ProjectZombie")
			                 {
			                 	LastBuildStatus = ProjectStatus.FAILURE,
			                 };
		}

		[Test]
		public void CanParseSentence_WithVariables_ProjectName_And_Breaker()
		{
			_buildBuster.Stub(b => b.FindBreaker(Arg<string>.Is.Anything)).Return("PandaWood");

			var parsedSentence = _speechTextParser.Parse(
				"$ProjectName$, is broken.$Breaker$, you're fired!", _failedProject);

			parsedSentence.ShouldBe("ProjectZombie, is broken.PandaWood, you're fired!");
		}

		[Test]
		public void CanParse_Sentence_With_NoVariables_ForBackwardsCompatibility_And_Minimalism()
		{
			_buildBuster.Stub(b => b.FindBreaker(Arg<string>.Is.Anything)).Return("PandaWood");

			var parsedSentence = _speechTextParser.Parse("is broken", _failedProject);

			parsedSentence.ShouldBe("ProjectZombie, is broken");
		}
	}
}