using System;
using System.Linq;
using Cradiator.Extensions;
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
		public void invalid_if_uri_emptystring()
		{
			var cruiseAddress = new CruiseAddress("");
			cruiseAddress.Invalid.ShouldBe(true);
		}

		[Test]
		public void valid_if_url_valid()
		{
			var cruiseAddress = new CruiseAddress("http://valid");
			cruiseAddress.Valid.ShouldBe(true);
		}

		[Test]
		public void valid_debug()
		{
			var cruiseAddress = new CruiseAddress("debug");
			cruiseAddress.Valid.ShouldBe(true);
			cruiseAddress.IsDebug.ShouldBe(true);
		}

        [Test]
        public void multi_uris()
        {
            var cruiseAddress = new CruiseAddress("http://bla1|http://bla2");
            cruiseAddress.Valid.ShouldBe(true);
            cruiseAddress.IsDebug.ShouldBe(false);
            cruiseAddress.UriList.Count().ShouldBe(2);
            cruiseAddress.UriList.First().ShouldBe(new Uri("http://bla1"));
            cruiseAddress.UriList.Second().ShouldBe(new Uri("http://bla2"));
        }
	}
}