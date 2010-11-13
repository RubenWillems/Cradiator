using System;
using System.Collections.Generic;
using System.Linq;
using Cradiator.Config;
using Ninject;

namespace Cradiator.Model
{
	public class ViewUrl
	{
		private readonly UrlParser _urlParser;

		// ReSharper disable UnusedMember.Global
		[Inject]
		public ViewUrl(IConfigSettings settings) : this(settings.URL) {}
		// ReSharper restore UnusedMember.Global

		public ViewUrl(string url)
		{
			Url = url;
			_urlParser = new UrlParser(Url);
		}

		public string Url { get; set; }

		public Uri Uri
		{
			get { return _urlParser.Uri; }
		}

		public IEnumerable<Uri> UriList
		{
			get
			{
				return from url in Url.Split(' ')
				       let u = new UrlParser(url)
				       where u.IsValid
				       select u.Uri;
			}
		}
	}
}