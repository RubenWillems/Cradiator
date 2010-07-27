using System;

namespace Cradiator.Model
{
	public class FetchException : Exception
	{
		private readonly string _url;

		public FetchException(string url, Exception exception)
			: base("Unable to contact " + url, exception)
		{
			_url = url;
		}

		public FetchException(string url)
			: base(string.Format("Invalid URL '{0}'", url))
		{
			_url = url;
		}

		public string Url
		{
			get { return _url; }
		}
	}
}