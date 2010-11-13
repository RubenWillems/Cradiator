using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cradiator.Config;
using Cradiator.Services;

namespace Cradiator.Model
{
	public class BuildDataFetcher : IConfigObserver
	{
		readonly ViewUrl _viewUrl;
		readonly IWebClientFactory _webClientFactory;
		IWebClient _webClient;

		public BuildDataFetcher(ViewUrl viewUrl, IConfigSettings configSettings,
								IWebClientFactory webClientFactory)
		{
			_viewUrl = viewUrl;
			_webClientFactory = webClientFactory;
			_webClient = webClientFactory.GetWebClient(configSettings.URL);
			configSettings.AddObserver(this);
		}

		public IEnumerable<string> Fetch()
		{
			try
			{
				return _viewUrl.UriList.Select(uri => _webClient.DownloadString(uri)).ToList();
			}
			catch (WebException webException)
			{   //todo will this identify the specific uri attempted
				throw new FetchException(_viewUrl.Url, webException);
			}
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_viewUrl.Url = newSettings.URL;
			_webClient = _webClientFactory.GetWebClient(newSettings.URL);
		}
	}
}