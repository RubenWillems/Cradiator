using System;
using System.Collections.Generic;
using System.Speech.Synthesis;
using Cradiator.Config;
using Cradiator.Extensions;
using Cradiator.Model;

namespace Cradiator.Audio
{
	public class SpeechMaker : IConfigObserver
	{
		readonly TimeSpan OneSecond = new TimeSpan(0, 0, 1);

		readonly ISpeechTextParser _speechTextParser;
		string _brokenBuildText;
		string _fixedBuildText;

		public SpeechMaker(IConfigSettings configSettings, ISpeechTextParser speechTextParser)
		{
			_speechTextParser = speechTextParser;
			_brokenBuildText = configSettings.BrokenBuildText;
			_fixedBuildText = configSettings.FixedBuildText;
			configSettings.AddObserver(this);
		}

		public PromptBuilder BuildIsBroken(IEnumerable<ProjectStatus> projects)
		{
			return MakeSpeech(projects, _brokenBuildText);
		}

		public PromptBuilder BuildIsFixed(IEnumerable<ProjectStatus> projects)
		{
			return MakeSpeech(projects, _fixedBuildText);
		}

		PromptBuilder MakeSpeech(IEnumerable<ProjectStatus> projects, string rawSentence)
		{
			var promptBuilder = new PromptBuilder();

			if (rawSentence.IsEmpty()) return promptBuilder;

			promptBuilder.AppendBreak(OneSecond);
			foreach (var project in projects)
			{
				promptBuilder.AppendBreak(OneSecond);
				promptBuilder.AppendText(_speechTextParser.Parse(rawSentence, project));
			}
			return promptBuilder;
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_brokenBuildText = newSettings.BrokenBuildText;
			_fixedBuildText = newSettings.FixedBuildText;
		}
	}
}