using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cradiator.Config;
using Cradiator.Services;

namespace Cradiator.Model
{
	public class BuildDataFetcher : IConfigObserver
	{
		readonly CradiatorUrl _cradiatorUrl;
		readonly IWebClientFactory _webClientFactory;
		IWebClient _webClient;

		public BuildDataFetcher(CradiatorUrl cradiatorUrl, IConfigSettings configSettings,
		                        IWebClientFactory webClientFactory)
		{
			_cradiatorUrl = cradiatorUrl;
			_webClientFactory = webClientFactory;
			_webClient = webClientFactory.GetWebClient(configSettings.URL);
			configSettings.AddObserver(this);
		}

		public IEnumerable<string> Fetch()
		{
			if (_cradiatorUrl.IsNotValid) throw new FetchException(_cradiatorUrl.Url);

			try
			{
			    return _cradiatorUrl.UriList.Select(uri => _webClient.DownloadString(uri)).ToList();
			}
			catch (WebException webException)
            {   //todo will this identify the specific uri attempted
				throw new FetchException(_cradiatorUrl.Url, webException);
			}
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_cradiatorUrl.Url = newSettings.URL;
			_webClient = _webClientFactory.GetWebClient(newSettings.URL);
		}
	}
}