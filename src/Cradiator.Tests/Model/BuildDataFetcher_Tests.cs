using System;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Services;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class BuildDataFetcher_Tests
	{
		IWebClientFactory _webClientFactory;
		IWebClient _webClient;

		[SetUp]
		public void SetUp()
		{
			_webClientFactory = Create.Stub<IWebClientFactory>();
			_webClient = Create.Mock<IWebClient>();
			_webClientFactory.Stub(w => w.GetWebClient(Arg<string>.Is.Anything)).Return(_webClient);
		}

		[Test]
		public void CanFetch()
		{
			const string Hello = "hello";

			_webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return(Hello);

			var fetcher = new BuildDataFetcher(new CruiseAddress("http://test"), new ConfigSettings(), _webClientFactory);
			var fetchValue = fetcher.Fetch();

			Assert.That(fetchValue, Is.EqualTo(Hello));
		}

		[Test]
		public void CanUpdateSettings()
		{
			var fetcher = new BuildDataFetcher(new CruiseAddress("http://bla"), 
				new ConfigSettings
				{
					URL = "http://bla"
				}, _webClientFactory);

			fetcher.Fetch();
			_webClient.AssertWasCalled(w => w.DownloadString(new Uri("http://bla")), w=>w.Repeat.Once());
			_webClient.AssertWasNotCalled(w => w.DownloadString(new Uri("http://new")));

			fetcher.ConfigUpdated(new ConfigSettings { URL = "http://new"});
			fetcher.Fetch();
			_webClient.AssertWasCalled(w => w.DownloadString(new Uri("http://new")), w=>w.Repeat.Once());
		}

		[Test]
		public void CanThrow_If_Invalid()
		{
			var fetcher = new BuildDataFetcher(new CruiseAddress(""), new ConfigSettings(), _webClientFactory);
			Assert.Throws<FetchException>(() => fetcher.Fetch());
		}
	}
}