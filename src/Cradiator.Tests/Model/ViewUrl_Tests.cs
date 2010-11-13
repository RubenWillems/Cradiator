using System.Linq;
using Cradiator.Model;
using NUnit.Framework;
using Shouldly;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class ViewUrl_Tests
	{
		[Test]
		public void can_append_filename_if_doesnt_exist()
		{
			var url = new ViewUrl("http://mycruise/ccnet");

			url.Uri.ToString().ShouldContain("/XmlStatusReport.aspx");
			url.Uri.ToString().ShouldContain("/XmlStatusReport.aspx");
		}

		[Test]
		public void doesnot_append_filename_if_already_exists()
		{
			var url = new ViewUrl("http://mycruise/ccnet/XmlStatusReport.aspx");

			url.Uri.ToString().ShouldContain("mycruise/ccnet/XmlStatusReport.aspx");
		}

		[Test]
		public void doesnot_append_filename_if_already_exists_or_not_ccnet()
		{
			var url = new ViewUrl("http://www.spice-3d.org/cruise/xml");

			url.Uri.ToString().ShouldContain("www.spice-3d.org/cruise/xml");
		}

		[Test]
		public void doesnot_prepend_http_if_already_exists()
		{
			var url = new ViewUrl("http://mycruise/ccnet");
			url.Uri.ToString().ShouldContain("http://mycruise/ccnet");
		}

		[Test]
		public void invalid_if_uri_emptystring()
		{
			var url = new UrlParser("");
			url.IsNotValid.ShouldBe(true);
		}

		[Test]
		public void invalid_uri_2()
		{
			var url = new UrlParser("pp");
			url.IsNotValid.ShouldBe(true);
		}

		[Test]
		public void invalid_uri_3()
		{
			var url = new UrlParser("debung");
			url.IsNotValid.ShouldBe(true);
		}

		[Test]
		public void invalid_uri_5()
		{
			var url = new UrlParser("bla.aspx");
			url.IsNotValid.ShouldBe(true);
		}

		[Test]
		public void valid_url_1()
		{
			var url = new UrlParser("http://valid.com");
			url.IsValid.ShouldBe(true);
		}

		[Test]
		public void valid_url_2()
		{
			var url = new UrlParser("http://mycruise/ccnet/XmlStatusReport.aspx");
			url.IsValid.ShouldBe(true);
		}

		[Test]
		public void valid_url_3()
		{
			var url = new UrlParser("http://ccnetlive.thoughtworks.com/ccnet/XmlStatusReport.aspx");
			url.IsValid.ShouldBe(true);
		}

		[Test]
		public void valid_url_4()
		{
			var url = new UrlParser("http://localhost/ccnet/XmlStatusReport.aspx?server=5");
			url.IsValid.ShouldBe(true);
		}

		[Test]
		public void valid_debug()
		{
			var url = new UrlParser("debug");
			url.IsValid.ShouldBe(true);
			url.IsDebug.ShouldBe(true);
		}

		[Test]
		public void multi_uris_2()
		{
			var url = new ViewUrl("http://www.3d.org/cruise/xml http://bla2.com/xml");
			url.UriList.Count().ShouldBe(2);
		}

		[Test]
		public void multi_uris_3()
		{
			var url = new ViewUrl("http://www.3d.org/cruise/xml http://bla2.com/xml http://bla3.com/xml");
			url.UriList.Count().ShouldBe(3);
		}

		[Test]
		public void urilist_filters_invalid()
		{
			var url = new ViewUrl("poop http://cc/xml.aspx");
			url.UriList.Count().ShouldBe(1);
		}
	}
}