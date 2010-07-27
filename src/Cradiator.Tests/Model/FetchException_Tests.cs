using Cradiator.Model;
using NUnit.Framework;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class FetchException_Tests
	{
		[Test]
		public void check_that_the_uri_is_accessible_when_the_dashboard_exception_is_thrown()
		{
			var exception = new FetchException("http://www.foo.com", null);
			Assert.That(exception.Url, Is.EqualTo("http://www.foo.com"));
		}
	}
}