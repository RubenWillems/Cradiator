using System.Collections.Generic;
using System.Linq;
using System.Net;
using Cradiator.Config;
using Cradiator.Services;

namespace Cradiator.Model
{
	public class BuildDataFetcher : IConfigObserver
	{
		readonly CruiseAddress _cruiseAddress;
		readonly IWebClientFactory _webClientFactory;
		IWebClient _webClient;

		public BuildDataFetcher(CruiseAddress cruiseAddress, IConfigSettings configSettings,
		                        IWebClientFactory webClientFactory)
		{
			_cruiseAddress = cruiseAddress;
			_webClientFactory = webClientFactory;
			_webClient = webClientFactory.GetWebClient(configSettings.URL);
			configSettings.AddObserver(this);
		}

		public IEnumerable<string> Fetch()
		{
			if (_cruiseAddress.IsNotValid) throw new FetchException(_cruiseAddress.Url);

			try
			{
			    return _cruiseAddress.UriList.Select(uri => _webClient.DownloadString(uri)).ToList();
			}
			catch (WebException webException)   //todo will this identify the specific uri attempted
			{
				throw new FetchException(_cruiseAddress.Url, webException);
			}
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			_cruiseAddress.Url = newSettings.URL;
			_webClient = _webClientFactory.GetWebClient(newSettings.URL);
		}
	}
}