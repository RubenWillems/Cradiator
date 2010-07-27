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

		public string Fetch()
		{
			if (!_cruiseAddress.IsValid) throw new FetchException(_cruiseAddress.Url);

			try
			{
				return _webClient.DownloadString(_cruiseAddress.Uri);
			}
			catch (WebException webException)
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