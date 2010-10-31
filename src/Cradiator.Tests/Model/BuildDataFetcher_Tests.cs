using System;
using System.Linq;
using Cradiator.Config;
using Cradiator.Extensions;
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

			fetchValue.First().ShouldBe(Hello);
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
			_webClient.AssertWasCalled(w => w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://bla"))), w=>w.Repeat.Once());

			fetcher.ConfigUpdated(new ConfigSettings { URL = "http://new"});
			fetcher.Fetch();
            _webClient.AssertWasCalled(w => w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://new"))), w => w.Repeat.Once());
		}

		[Test]
		public void CanThrow_If_Invalid()
		{
			var fetcher = new BuildDataFetcher(new CruiseAddress(""), new ConfigSettings(), _webClientFactory);
			Assert.Throws<FetchException>(() => fetcher.Fetch());
		}

        [Test]
        public void can_fetch_multi()
        {
            _webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return("url1").Repeat.Once();
            _webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return("url2").Repeat.Once();

            var fetcher = new BuildDataFetcher(new CruiseAddress("http://url1|http://url2"), 
                new ConfigSettings(), _webClientFactory);

            var xmlResults = fetcher.Fetch().ToList();

            xmlResults.Count.ShouldBe(2);
            xmlResults[0].ShouldBe("url1");
            xmlResults[1].ShouldBe("url2");

            _webClient.AssertWasCalled(w=>w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://url1"))));
            _webClient.AssertWasCalled(w=>w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://url2"))));
        }

        [Test]
        public void can_fetch_multi_with_alternate_split_chars()
        {
            _webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return("url1").Repeat.Once();
            _webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return("url2").Repeat.Once();
            _webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return("url3").Repeat.Once();
            _webClient.Expect(w => w.DownloadString(Arg<Uri>.Is.Anything)).Return("url4").Repeat.Once();

            var fetcher = new BuildDataFetcher(new CruiseAddress("http://url1|http://url2 http://url3;http://url4"),
                new ConfigSettings(), _webClientFactory);

            var xmlResults = fetcher.Fetch().ToList();

            xmlResults.Count.ShouldBe(4);
            xmlResults[0].ShouldBe("url1");
            xmlResults[1].ShouldBe("url2");
            xmlResults[2].ShouldBe("url3");
            xmlResults[3].ShouldBe("url4");

            _webClient.AssertWasCalled(w => w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://url1"))));
            _webClient.AssertWasCalled(w => w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://url2"))));
            _webClient.AssertWasCalled(w => w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://url3"))));
            _webClient.AssertWasCalled(w => w.DownloadString(Arg<Uri>.Is.Equal(new Uri("http://url4"))));
        }
	}
}