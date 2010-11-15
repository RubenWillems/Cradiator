using System;
using System.Text.RegularExpressions;
using Cradiator.Extensions;
using log4net;

namespace Cradiator.Model
{
	public class UrlParser
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(ViewUrl).Name);
		private readonly string _url;

		public UrlParser(string url)
		{
			_url = url;
		}

		public string Url { get { return _url; } }

		public bool IsValid
		{
			get
			{

				var isValid = IsDebug ||
								Regex.IsMatch(_url, @"^((https?)://+[\w\d:#@%/;$()~_?\+-=\\\.&]*)");

				if (!isValid) _log.WarnFormat("Skipping invalid URL: '{0}'", _url);
				return isValid;
			}
		}

		public bool IsNotValid
		{
			get { return !IsValid; }
		}

		public bool IsDebug
		{
			get { return _url.HasValue() && _url.ToLower().StartsWith("debug"); }
		}
	}
}