using System.Text.RegularExpressions;
using Cradiator.MigrateConfig;
using NUnit.Framework;

namespace Cradiator.Tests.MigrateConfig
{
	[TestFixture]
	public class Migrate_Tests
	{
		private const string PRE_MULTIVIEW_XML =
			@"<?xml version=""1.0""?>
<configuration>
<configSections>
<section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net""/>
<section name=""usernames"" type=""System.Configuration.NameValueSectionHandler""/>
</configSections>
<appSettings>
<add key=""URL"" value=""debughttp://ccnetlive.thoughtworks.com/ccnet""/>
<add key=""Skin"" value=""StackPhoto""/>
<add key=""PollFrequency"" value=""30""/>
<add key=""ProjectNameRegEx"" value="".*""/>
<add key=""CategoryRegEx"" value="".*""/>
<add key=""ShowCountdown"" value=""true""/>
<add key=""ShowProgress"" value=""true""/>
<add key=""PlaySounds"" value=""true""/>
<add key=""BrokenBuildSound"" value=""explosion.wav""/>
<add key=""FixedBuildSound"" value=""AppeggioOfSuccess.wav""/>
<add key=""PlaySpeech"" value=""true""/>
<add key=""BrokenBuildText"" value=""$ProjectName$ is broken. $Breaker$""/>
<add key=""FixedBuildText"" value=""$ProjectName$ is fixed""/>
<add key=""SpeechVoiceName"" value=""""/>
<add key=""BreakerGuiltStrategy"" value=""Last""/>
</appSettings>
<usernames>
<add key=""jsmith"" value=""John Smith""/>
</usernames>
<log4net>
<appender name=""RollingLogFileAppender"" type=""log4net.Appender.RollingFileAppender"">
<file value=""cradiator.log""/>
<appendToFile value=""true""/>
<rollingStyle value=""Size""/>
<datePattern value=""yyyy-MM-dd""/>
<maxSizeRollBackups value=""1""/>
<maximumFileSize value=""1MB""/>
<layout type=""log4net.Layout.PatternLayout"">
<conversionPattern value=""%date %-5level %logger - %message%newline""/>
</layout>
</appender>
<root>
<level value=""INFO""/>
<appender-ref ref=""RollingLogFileAppender""/>
</root>
</log4net>
<startup/>
</configuration>";

		[Test]
		public void can_migrate()
		{
			var migrate = new MultiviewMigrator(PRE_MULTIVIEW_XML);
			var updatedXml = migrate.Migrate();
			var strippedXml = Regex.Replace(updatedXml, @"[\r\n\t\s]", "");
			const string expected = @"<configuration>
<configSections>
<section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net""/>
<section name=""usernames"" type=""System.Configuration.NameValueSectionHandler""/>
<section name=""views"" type=""System.Configuration.IgnoreSectionHandler""/>
</configSections>
<views>
<view url=""debughttp://ccnetlive.thoughtworks.com/ccnet"" skin=""StackPhoto"" project-regex="".*"" category-regex="".*"" />
</views>";
			var strippedExpected = Regex.Replace(expected, @"[\r\n\t\s]", "");

			Assert.That(strippedXml, Is.StringStarting(strippedExpected));
		}
	}
}