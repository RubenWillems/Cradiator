using Cradiator.Model;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class CruiseAddress_Tests
	{
		[Test]
		public void can_append_filename_if_doesnt_exist()
		{
			var cruiseAddress = new CruiseAddress("http://mycruise/ccnet");

            cruiseAddress.Uri.ToString().ShouldContain("/XmlStatusReport.aspx");
			cruiseAddress.Uri.ToString().ShouldContain("/XmlStatusReport.aspx");
		}

		[Test]
		public void doesnot_append_filename_if_already_exists()
		{
			var cruiseAddress = new CruiseAddress("http://mycruise/ccnet/XmlStatusReport.aspx");

            cruiseAddress.Uri.ToString().ShouldContain("mycruise/ccnet/XmlStatusReport.aspx");
		}

		[Test]
		public void doesnot_append_filename_if_already_exists_or_not_ccnet()
		{
			var cruiseAddress = new CruiseAddress("http://www.spice-3d.org/cruise/xml");

			cruiseAddress.Uri.ToString().ShouldContain("www.spice-3d.org/cruise/xml");
		}

		[Test]
		public void doesnot_prepend_http_if_already_exists()
		{
			var cruiseAddress = new CruiseAddress("http://mycruise/ccnet");
			cruiseAddress.Uri.ToString().ShouldContain("http://mycruise/ccnet");
		}

		[Test]
		public void isvalid_false_if_uri_emptystring()
		{
			var cruiseAddress = new CruiseAddress("");
			cruiseAddress.IsValid.ShouldBe(false);
		}

		[Test]
		public void isvalid_true_if_url_valid()
		{
			var cruiseAddress = new CruiseAddress("http://valid");
			cruiseAddress.IsValid.ShouldBe(true);
		}

		[Test]
		public void isvalid_if_debug()
		{
			var cruiseAddress = new CruiseAddress("debug");
			cruiseAddress.IsValid.ShouldBe(true);
			cruiseAddress.IsDebug.ShouldBe(true);
		}
	}
}