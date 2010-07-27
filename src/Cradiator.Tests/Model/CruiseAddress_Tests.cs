using Cradiator.Model;
using NUnit.Framework;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class CruiseAddress_Tests
	{
		[Test]
		public void can_append_filename_if_doesnt_exist()
		{
			var cruiseAddress = new CruiseAddress("http://mycruise/ccnet");

			Assert.That(cruiseAddress.Uri.ToString(), Text.EndsWith("/XmlStatusReport.aspx"));
		}

		[Test]
		public void doesnot_append_filename_if_already_exists()
		{
			var cruiseAddress = new CruiseAddress("http://mycruise/ccnet/XmlStatusReport.aspx");

			Assert.That(cruiseAddress.Uri.ToString(), Text.EndsWith("mycruise/ccnet/XmlStatusReport.aspx"));
		}

		[Test]
		public void doesnot_append_filename_if_already_exists_or_not_ccnet()
		{
			var cruiseAddress = new CruiseAddress("http://www.spice-3d.org/cruise/xml");

			Assert.That(cruiseAddress.Uri.ToString(), Text.EndsWith("www.spice-3d.org/cruise/xml"));
		}

		[Test]
		public void doesnot_prepend_http_if_already_exists()
		{
			var cruiseAddress = new CruiseAddress("http://mycruise/ccnet");
			Assert.That(cruiseAddress.Uri.ToString(), Text.StartsWith("http://mycruise/ccnet"));
		}

		[Test]
		public void isvalid_false_if_uri_emptystring()
		{
			var cruiseAddress = new CruiseAddress("");
			Assert.That(cruiseAddress.IsValid, Is.False);
		}

		[Test]
		public void isvalid_true_if_url_valid()
		{
			var cruiseAddress = new CruiseAddress("http://valid");
			Assert.That(cruiseAddress.IsValid, Is.True);
		}

		[Test]
		public void isvalid_if_debug()
		{
			var cruiseAddress = new CruiseAddress("debug");
			Assert.That(cruiseAddress.IsValid, Is.True);
			Assert.That(cruiseAddress.IsDebug, Is.True);
		}
	}
}