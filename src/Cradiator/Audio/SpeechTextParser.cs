using System.Linq;
using Cradiator.Model;

namespace Cradiator.Audio
{
	public interface ISpeechTextParser 
	{
		string Parse(string sentence, ProjectStatus projectStatus);
	}

	public class SpeechTextParser : ISpeechTextParser
	{
		readonly IBuildBuster _buildBuster;

		public SpeechTextParser([InjectBuildBusterFullNameDecorator] IBuildBuster buildBuster)
		{
			_buildBuster = buildBuster;
		}

		public string Parse(string sentence, ProjectStatus projectStatus)
		{
			var reservedWords =
				from word in sentence.Split(new[] {' ', ',', '.'})
				where word.StartsWith("$")
				where word.EndsWith("$")
				select word;

			if (reservedWords.Any())
			{
				foreach (var word in reservedWords)
				{
					if (word == "$ProjectName$")
						sentence = sentence.Replace(word, projectStatus.Name);

					if (word == "$Breaker$")
						sentence = sentence.Replace(word, _buildBuster.FindBreaker(projectStatus.CurrentMessage));
				}
			}
			else
			{
				sentence = string.Format("{0}, {1}", projectStatus.Name, sentence);
			}

			return sentence;
		}
	}
}