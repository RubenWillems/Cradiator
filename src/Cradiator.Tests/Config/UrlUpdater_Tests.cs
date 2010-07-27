using BigVisibleCruise2.Config;
using BigVisibleCruise2.Model;
using BigVisibleCruise2.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace BigVisibleCruise2.Tests.Config
{
	[TestFixture]
	public class UrlUpdater_Tests
	{
		IConfigObserver _urlUpdateResponder;
		IWebClientFactory _webClientFactory;
		IConfigSettings _configSettings;
		IWebClient _webClient;

		[SetUp]
		public void SetUp()
		{
			_webClientFactory = MockRepository.GenerateMock<IWebClientFactory>();
			_webClient = MockRepository.GenerateStub<IWebClient>();
			
			_webClientFactory.Expect(w => w.GetWebClient(Arg<string>.Is.Anything)).Return(_webClient).Repeat.Once();

			_configSettings = new ConfigSettings { ProjectNameRegEx = ".*" };
			_urlUpdateResponder = new UrlUpdateResponder(
				new StatusFetcher(new CruiseAddress("url"), 
					_configSettings, _webClientFactory), _configSettings, _webClientFactory);

			_configSettings.URL = "currentUrl";
		}

		[Test]
		public void CanChange_Url()
		{
			_configSettings.URL = "oldURL";
			_urlUpdateResponder.ConfigUpdated(new ConfigSettings { URL = "newURL" });

			_webClientFactory.AssertWasCalled(w=>w.GetWebClient("newURL"));
		}

		[Test]
		public void DoesntChangeUrl_IfNotDifferent()
		{
			_configSettings.URL = "sameURL";
			_urlUpdateResponder.ConfigUpdated(new ConfigSettings { URL = "sameURL" });

			_webClientFactory.AssertWasNotCalled(w => w.GetWebClient("sameURL"));
		}
	}
}