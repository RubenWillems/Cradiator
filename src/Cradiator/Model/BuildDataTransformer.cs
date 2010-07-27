using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Cradiator.Config;
using Cradiator.Extensions;

namespace Cradiator.Model
{
	public interface IBuildDataTransformer
	{
		IEnumerable<ProjectStatus> Transform(string xml);
	}

	public class BuildDataTransformer : IConfigObserver, IBuildDataTransformer
	{
		Regex _projectNameRegEx;
		Regex _categoryRegEx;

		public BuildDataTransformer(IConfigSettings configSettings)
		{
			SetRegex(configSettings);
			configSettings.AddObserver(this);
		}

		public IEnumerable<ProjectStatus> Transform(string xml)
		{
			if (xml.IsEmpty()) return new List<ProjectStatus>();

			// caters for ccNet v1.5 new xml format for CurrentMessage (messages/message)
			return (from p in (from project in XDocument.Parse(xml.Replace('\n',' ').Trim())
			                   	.Elements("Projects")
			                   	.Elements("Project")
			                   let name = project.Attribute("name").GetValue()
			                   let category = project.Attribute("category").GetValue()
			                   where _projectNameRegEx.Match(name).Success
			                   where _categoryRegEx.Match(category).Success
			                   select new ProjectStatus(name)
			                          {
			                          	CurrentMessage = project.Attribute("CurrentMessage").GetValue(),
			                          	LastBuildStatus = project.Attribute("lastBuildStatus").GetValue(),
			                          	ProjectActivity = new ProjectActivity(project.Attribute("activity").GetValue())
			                          })
			        join m in (from message in XDocument.Parse(xml)
			                   	.Elements("Projects")
			                   	.Elements("Project")
			                   	.Elements("messages")
			                   	.Elements("message")
			                   where message.Attribute("kind").GetValue() == "Breakers"
			                   select new
			                          {
			                          	Message = message.Attribute("text").GetValue(),
			                          	ProjectName = message.Parent.Parent.Attribute("name").GetValue(),
			                          }) on p.Name equals m.ProjectName into j
			        from m in j.DefaultIfEmpty()
			        select new ProjectStatus(p.Name)
			               {
			               	CurrentMessage = m != null ? m.Message : p.CurrentMessage,
			               	LastBuildStatus = p.LastBuildStatus,
			               	ProjectActivity = p.ProjectActivity
			               });
		}

		public void ConfigUpdated(ConfigSettings newSettings)
		{
			SetRegex(newSettings);
		}

		void SetRegex(IConfigSettings newSettings)
		{
			_projectNameRegEx = new Regex(newSettings.ProjectNameRegEx);
			_categoryRegEx = new Regex(newSettings.CategoryRegEx);
		}
	}
}