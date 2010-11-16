using Cradiator.Model;
using log4net;

namespace Cradiator.Services
{
	public interface IWebClientFactory
	{
		IWebClient GetWebClient(string url);
	}

	public class WebClientFactory : IWebClientFactory
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(WebClientFactory).Name);

		public IWebClient GetWebClient(string url)
		{
			IWebClient webClient;

			var uri = new UrlParser(url);
			if (uri.IsDebug)
			{
				webClient = new SandboxWebClient();
			}
			else
			{
				webClient = new HttpWebClient();
			}

			_log.DebugFormat("Using WebClient: {0}", webClient.GetType());
			return webClient;
		}
	}
}