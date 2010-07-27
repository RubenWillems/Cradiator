using System.Linq;
using Cradiator.Config;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Config
{
	[TestFixture]
	public class UsernameMappingReader_Tests
	{
		UserNameMappingReader _reader;
		IConfigLocation _configLocation;

		[SetUp]
		public void SetUp()
		{
			_configLocation = MockRepository.GenerateStub<IConfigLocation>();

			_reader = new UserNameMappingReader(_configLocation)
			{
				Xml = @"<configuration><usernames><add key=""jsmith"" value=""John Smith""/></usernames></configuration>"
			};
		}

		[Test]
		public void CanParse_Usernames_From_ConfigFileXml()
		{
			var usernameMappings = _reader.Read();
			var jsmiths = usernameMappings.Where(u => u.Key == "jsmith");

			Assert.That(jsmiths.Any());
			Assert.That(jsmiths.First().Value, Is.EqualTo("John Smith"));
		}
	}
}