using System;
using System.IO;

namespace Cradiator.Services
{
	public class SandboxWebClient : IWebClient
	{
		/// <summary>
		/// Return the project file 'DummyProjectStatus.xml' as a string
		/// This xml file is copied to OutDir as part of the build automatically 
		/// It's used to test/play without having to connect to a real web service
		/// </summary>
		/// <param name="uri">ignored in this implementation</param>
		public string DownloadString(string url)
		{
			using (var streamReader = new StreamReader("DummyProjectStatus.xml"))
			{
				return streamReader.ReadToEnd();
			}
		}
	}
}