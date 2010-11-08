using System;

namespace Cradiator.Services
{
	public interface IWebClient
	{
		string DownloadString(Uri uri);
	}
}