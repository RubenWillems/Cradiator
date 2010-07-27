using System;
using Cradiator.Config;
using Cradiator.Extensions;
using Ninject;

namespace Cradiator.Model
{
	public class CruiseAddress
	{
		[Inject]
		public CruiseAddress(IConfigSettings settings)
		{
			Url = settings.URL;
		}

		public CruiseAddress(string url)
		{
			Url = url;
		}

		public string Url { get; set; }

		public Uri Uri
		{
			get 
			{
				var uri = Url.Trim();

				if (uri.Contains("ccnet") && !uri.EndsWith("/XmlStatusReport.aspx"))
					uri += "/XmlStatusReport.aspx";

				return new Uri(uri);
			}
		}

		public bool IsValid
		{
			get { return !Url.IsEmpty(); }
		}

		public bool IsDebug
		{
			get { return IsValid && Url.ToLower().StartsWith("debug"); }
		}
	}
}