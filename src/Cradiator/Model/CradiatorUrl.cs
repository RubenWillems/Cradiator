using System;
using System.Collections.Generic;
using System.Linq;
using Cradiator.Config;
using Cradiator.Extensions;
using Ninject;

namespace Cradiator.Model
{
	public class CradiatorUrl
	{
	    [Inject]
		public CradiatorUrl(IConfigSettings settings)
		{
			Url = settings.URL;
		}

		public CradiatorUrl(string url)
		{
			Url = url;
		}

		public string Url { get; set; }

        public IEnumerable<Uri> UriList
        {
            get
            {
                return from url in Url.Split(' ')
                       let ad = new CradiatorUrl(url)
                       where ad.IsValid
                       select ad.Uri;
            }
        }

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

        public bool IsNotValid
        {
            get { return !IsValid; }
        }

		public bool IsDebug
		{
			get { return IsValid && Url.ToLower().StartsWith("debug"); }
		}
	}
}