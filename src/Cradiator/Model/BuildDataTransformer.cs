using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Cradiator.Config;
using Cradiator.Extensions;
using System;
using log4net;

namespace Cradiator.Model
{
    public class BuildDataTransformer : IConfigObserver
    {
        Regex _projectNameRegEx;
        Regex _categoryRegEx;
        Regex _serverNameRegEx;

        public BuildDataTransformer(IConfigSettings configSettings)
        {
            SetLocalValuesFromConfig(configSettings);
            configSettings.AddObserver(this);
        }

        public IEnumerable<ProjectStatus> Transform(string xml)
        {
            if (xml.IsEmpty()) return new List<ProjectStatus>();

            // caters for ccNet v1.5 new xml format for CurrentMessage (messages/message)
            var query = (from p in
                             (from project in XDocument.Parse(xml.Replace('\n', ' ').Trim())
                               .Elements("Projects")
                               .Elements("Project")

                              let name = project.Attribute("name").GetValue()
                              let category = project.Attribute("category").GetValue()
                              let serverName = project.Attribute("serverName").GetValue()

                              where _projectNameRegEx.Match(name).Success
                              where _categoryRegEx.Match(category).Success
                              where _serverNameRegEx.Match(serverName).Success

                              select new ProjectStatus(name)
                              {
                                  CurrentMessage = project.Attribute("CurrentMessage").GetValue(),
                                  LastBuildStatus = project.Attribute("lastBuildStatus").GetValue(),
                                  ProjectActivity = new ProjectActivity(project.Attribute("activity").GetValue()),
                                  ServerName = project.Attribute("serverName").GetValue(),
                                  LastBuildTime = System.Xml.XmlConvert.ToDateTime(project.Attribute("lastBuildTime").GetValue(), System.Xml.XmlDateTimeSerializationMode.Local)
                              })
                         
                         join m in
                             (from message in XDocument.Parse(xml)
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
                             ProjectActivity = p.ProjectActivity,
                             ServerName = p.ServerName,
                             LastBuildTime = p.LastBuildTime
                         }
                         );


            return query.ToArray();
        }

        public void ConfigUpdated(ConfigSettings newSettings)
        {
            SetLocalValuesFromConfig(newSettings);
        }

        void SetLocalValuesFromConfig(IConfigSettings newSettings)
        {
            _projectNameRegEx = new Regex(newSettings.ProjectNameRegEx);
            _categoryRegEx = new Regex(newSettings.CategoryRegEx);
            _serverNameRegEx = new Regex(newSettings.ServerNameRegEx);
        }
    }
}