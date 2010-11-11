using System.Collections.Generic;
using System.Linq;
using Cradiator.Config;
using Cradiator.Model;

namespace Cradiator.Audio
{
	public class DiscJockey : IConfigObserver
	{
		IEnumerable<ProjectStatus> _previousBuildData = new List<ProjectStatus>();
		readonly IAudioPlayer _audioPlayer;
		readonly SpeechMaker _speechMaker;
		string _brokenBuildSound;
		string _fixedBuildSound;

		public DiscJockey(IConfigSettings configSettings, IAudioPlayer audioPlayer, SpeechMaker speechMaker)
		{
			_audioPlayer = audioPlayer;
			_speechMaker = speechMaker;
			_brokenBuildSound = configSettings.BrokenBuildSound;
			_fixedBuildSound = configSettings.FixedBuildSound;
			
			configSettings.AddObserver(this);
		}

		public void PlaySounds(IEnumerable<ProjectStatus> currentBuildData)
		{
			var newlyBrokenBuilds = 
				currentBuildData.Where(proj => proj.IsBroken)
								.Intersect(_previousBuildData.Where(proj => !proj.IsBroken));

			if (newlyBrokenBuilds.Any())
			{
				_audioPlayer.Play(_brokenBuildSound);
				_audioPlayer.Say(_speechMaker.BuildIsBroken(newlyBrokenBuilds));
			}
			else
			{
				var newlyFixedBuilds = 
					currentBuildData.Where(proj => proj.IsSuccessful)
									.Intersect(_previousBuildData.Where(proj => !proj.IsSuccessful));

				if (newlyFixedBuilds.Any())
				{
					_audioPlayer.Play(_fixedBuildSound);
					_audioPlayer.Say(_speechMaker.BuildIsFixed(newlyFixedBuilds));
				}
			}

			_previousBuildData = currentBuildData;
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_brokenBuildSound = newSettings.BrokenBuildSound;
			_fixedBuildSound = newSettings.FixedBuildSound;
		}
	}
}