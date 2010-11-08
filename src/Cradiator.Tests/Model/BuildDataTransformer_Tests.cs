using System.Linq;
using Cradiator.Config;
using Cradiator.Extensions;
using Cradiator.Model;
using NUnit.Framework;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class BuildDataTransformer_Tests
	{
		CradiatorUrl _cradiatorUrl;
		IConfigSettings _configSettings;
		BuildDataTransformer _transformer;

		[SetUp]
		public void SetUp()
		{
			_cradiatorUrl = new CradiatorUrl("http://valid/XmlStatusReport.aspx");
			_configSettings = new ConfigSettings { URL = _cradiatorUrl.Url };
			_transformer = new BuildDataTransformer(_configSettings);
		}

		[Test]
		public void status_is_building_if_building_and_last_status_was_failure()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' activity='Building' lastBuildStatus='Failure' />
				</Projects>";

			var projectStatuses = _transformer.Transform(xml);
			Assert.That(projectStatuses.First().CurrentState, Is.EqualTo(ProjectStatus.BUILDING));
		}

		[Test]
		public void status_is_building_if_building_and_last_status_was_success()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' activity='Building' lastBuildStatus='Success' />
				</Projects>";

			var projectStatuses = _transformer.Transform(xml);
			Assert.That(projectStatuses.First().CurrentState, Is.EqualTo(ProjectStatus.BUILDING));
		}

		[Test]
		public void status_is_failure_if_sleeping_and_last_status_was_failure()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' activity='Sleeping' lastBuildStatus='Failure' />
				</Projects>";

			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.First().CurrentState.EqualsIgnoreCase(ProjectStatus.FAILURE));
		}

		[Test]
		public void status_is_success_if_sleeping_and_last_status_was_success()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' activity='Sleeping' lastBuildStatus='Success' />
				  </Projects>";

			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.First().CurrentState.EqualsIgnoreCase(ProjectStatus.SUCCESS));
		}

		const string SimilarProjectXml =
			@"<Projects>
					<Project name='FooProject' activity='Sleeping' lastBuildStatus='Success' webUrl='http://foo/ccnet'/>
					<Project name='BarProject' activity='Sleeping' lastBuildStatus='Failure' webUrl='http://foo/ccnet'/>
					<Project name='FunProject' activity='Sleeping' lastBuildStatus='Failure' webUrl='http://foo/ccnet'/>
				</Projects>";

		[Test]
		public void CanFilter_ProjectName_With_BeginsWith_RegEx()
		{
			_configSettings.ProjectNameRegEx = "^F.*";
			_transformer = new BuildDataTransformer(_configSettings);

			var projectStatuses = _transformer.Transform(SimilarProjectXml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("FooProject"));
			Assert.That(projectStatuses.Second().Name, Is.EqualTo("FunProject"));
		}

		[Test]
		public void CanFilter_ProjectName_With_OR_RegEx()
		{
			_configSettings.ProjectNameRegEx = "FooProject|BarProject";
			_transformer = new BuildDataTransformer(_configSettings);

			var projectStatuses = _transformer.Transform(SimilarProjectXml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("FooProject"));
			Assert.That(projectStatuses.Second().Name, Is.EqualTo("BarProject"));
		}

		const string ThreeProjectsProjectXml =
			@"<Projects>
					<Project name='FooProject' category='' activity='Sleeping' lastBuildStatus='Success' lastBuildLabel='292' lastBuildTime='2007-11-16T15:03:46.358374-05:00' nextBuildTime='2007-11-16T15:31:00.2683768-05:00' webUrl='http://foo/ccnet'/>
					<Project name='BarProject' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='8' lastBuildTime='2007-11-16T05:00:00.2127436-05:00' nextBuildTime='2007-11-17T05:00:00-05:00' webUrl='http://foo/ccnet'/>
					<Project name='One_More_Project' category='' activity='Sleeping' lastBuildStatus='Failure' lastBuildLabel='39' lastBuildTime='2007-11-16T05:50:00.1105168-05:00' nextBuildTime='2007-11-17T05:50:00-05:00' webUrl='http://foo/ccnet'/>
				</Projects>";

		[Test]
		public void CanFilter_ProjectName_With_PlainProjectName_AsRegEx()
		{
			_configSettings.ProjectNameRegEx = "BarProject";
			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(ThreeProjectsProjectXml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("BarProject"));
		}

		[Test]
		public void CanTransform_MultipleProjects_WithNoFiltering()
		{
			var projectStatuses = _transformer.Transform(ThreeProjectsProjectXml);
			Assert.That(projectStatuses.Count(), Is.EqualTo(3));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("FooProject"));
			Assert.That(projectStatuses.Second().Name, Is.EqualTo("BarProject"));
			Assert.That(projectStatuses.Third().Name, Is.EqualTo("One_More_Project"));
		}

		[Test]
		public void RegExFilter_IsUpdated_After_ConfigUpdated()
		{
			_configSettings.ProjectNameRegEx = "FooProject|BarProject";
			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(SimilarProjectXml);
			Assert.That(projectStatuses.Count(), Is.EqualTo(2));

			// notify of config change and fetch again
			var newSettings = new ConfigSettings { ProjectNameRegEx = "BarProject", URL = _cradiatorUrl.Url };
			_transformer.ConfigUpdated(newSettings);
			projectStatuses = _transformer.Transform(SimilarProjectXml);
			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("BarProject"));
		}

		[Test]
		public void CurrentMessage_IsSet_IfPresent_InXml()
		{
			const string xml =
				@"<Projects>
					<Project name='FooProject' CurrentMessage='A message' category='' />
				</Projects>";

			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.First().CurrentMessage, Is.EqualTo("A message"));
		}

		[Test]
		public void CanHandleNull()
		{
			var projectStatuses = _transformer.Transform(null);

			Assert.That(projectStatuses.Count(), Is.EqualTo(0));
		}

		[Test]
		public void CanFilter_Category()
		{
			const string xml =
				@"<Projects>
					<Project name='ImportantProject' category='Important' />
					<Project name='LowPriorityProject' category='LowPriority'/>
				</Projects>";

			_configSettings.CategoryRegEx = "Important";
			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().Name, Is.EqualTo("ImportantProject"));
		}

		#region CCnet1.5 xml changes

		[Test]
		public void CanRead_CurrentMessage_FromNewStructure_In_CCnet15_With_1_FailingProject()
		{
			const string xml =
				@"<Projects>
					<Project name='ccnet1.5_project' activity='Sleeping' lastBuildStatus='Failure'>
						<messages>
							<message text='Breakers : a, b' kind='Breakers'/>
							<message text='FailingTasks : Step1, Step2' kind='FailingTasks'/>
						</messages>
					</Project>
				</Projects>";

			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(1));
			Assert.That(projectStatuses.First().CurrentMessage, Is.EqualTo("Breakers : a, b"));
		}

		[Test]
		public void CanRead_CurrentMessage_FromNewStructure_In_CCnet15_With_2_FailingProjects()
		{
			const string xml =
				@"<Projects>
					<Project name='project1' activity='Sleeping' lastBuildStatus='Failure'>
						<messages>
							<message text='Breakers : a, b' kind='Breakers'/>
							<message text='FailingTasks : Step1, Step2' kind='FailingTasks'/>
						</messages>
					</Project>
					<Project name='project2' activity='Sleeping' lastBuildStatus='Failure'>	
						<messages>
							<message text='Breakers : c, d' kind='Breakers'/>
							<message text='FailingTasks : Step1, Step2' kind='FailingTasks'/>
						</messages>
					</Project>
				</Projects>";

			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().CurrentMessage, Is.EqualTo("Breakers : a, b"));
			Assert.That(projectStatuses.Second().CurrentMessage, Is.EqualTo("Breakers : c, d"));
		}

		[Test]
		public void CanRead_CurrentMessage_FromNewStructure_In_CCnet15_With_NoFailing_Projects()
		{
			const string xml =
				@"<Projects>
					<Project name='project1' CurrentMessage='' activity='Sleeping' lastBuildStatus='Success'>
						<messages/>
					</Project>
					<Project name='project2' CurrentMessage='' activity='Sleeping' lastBuildStatus='Success'>	
						<messages/>							
					</Project>
				</Projects>";

			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().CurrentMessage, Is.EqualTo(""));
			Assert.That(projectStatuses.Second().CurrentMessage, Is.EqualTo(""));
		}

		[Test]
		public void CanRead_CurrentMessage_FromNewStructure_In_CCnet15_With_1_Failing_1_Success()
		{
			const string xml =
				@"<Projects>
					<Project name='project1' CurrentMessage='' activity='Sleeping' lastBuildStatus='Success'>
						<messages/>
					</Project>
					<Project name='project2' activity='Sleeping' CurrentMessage='' lastBuildStatus='Failure'>	
						<messages>
							<message text='Breakers : a, b' kind='Breakers'/>
							<message text='FailingTasks : Step1, Step2' kind='FailingTasks'/>
						</messages>
					</Project>
				</Projects>";

			_transformer = new BuildDataTransformer(_configSettings);
			var projectStatuses = _transformer.Transform(xml);

			Assert.That(projectStatuses.Count(), Is.EqualTo(2));
			Assert.That(projectStatuses.First().CurrentMessage, Is.EqualTo(""));
			Assert.That(projectStatuses.Second().CurrentMessage, Is.EqualTo("Breakers : a, b"));
		}

		#endregion
	}
}