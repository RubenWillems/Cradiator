using System.Linq;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Services;
using Cradiator.Views;
using Ninject;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[Ignore("Integration tests")]
	[Category("Integration")]
	[TestFixture]
	public class BuildStatusFetcher_IntegrationTests
	{
		ICradiatorView _view;
		IConfigSettings _configSettings;
		IKernel _kernel;
		IWebClientFactory _factory;

		[SetUp]
		public void SetUp()
		{
			_view = MockRepository.GenerateMock<ICradiatorView>();
			_configSettings = MockRepository.GenerateMock<IConfigSettings>();
			_configSettings.Expect(c => c.ProjectNameRegEx).Return(".*").Repeat.Any();
			_configSettings.Expect(c => c.CategoryRegEx).Return(".*").Repeat.Any();

			_kernel = new StandardKernel(new CradiatorNinjaModule(_view, _configSettings));
			_factory = _kernel.Get<IWebClientFactory>();
			_kernel.Get<BuildDataTransformer>();
		}

		[Test]
		public void a_connectivity_exception_will_be_thrown_if_the_uri_isnt_resolveable()
		{
			Assert.Throws<FetchException>(() =>
			{
			    var fetcher = new BuildDataFetcher(
			        new CruiseAddress("http://a.b.c.d.e.foo/ccnet/XmlStatusReport.aspx"), 
                        _configSettings, _factory);
                fetcher.Fetch();
			}, "Unable to contact http://a.b.c.d.e.foo/ccnet/XmlStatusReport.aspx");
		}

		/// <summary>
		/// this test is a little sensitive - and will fail if anything is going wrong at the thoughtworks URL
		/// </summary>
		[Test]
		public void can_get_projects_from_a_realaddress()
		{
			var fetcher = 
				new BuildDataFetcher(new CruiseAddress("http://ccnetlive.thoughtworks.com/ccnet/XmlStatusReport.aspx"),
				                     _configSettings, _factory);

		    var fetch = fetcher.Fetch();
		    fetch.Count().ShouldBeGreaterThan(0);
			fetch.ShouldContain(@"Project name=""CCNet""");
		}
	}
}