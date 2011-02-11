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
        static readonly ILog _log = LogManager.GetLogger(typeof(ScreenUpdater).Name);

        
        Regex _projectNameRegEx;
        Regex _categoryRegEx;
        bool _showOnlyBroken;
        string _viewName;


        public BuildDataTransformer(IConfigSettings configSettings)
        {
            SetLocalValuesFromConfig(configSettings);
            configSettings.AddObserver(this);
        }

        public ViewData Transform(string xml)
        {
            var result = new ViewData(_viewName, _showOnlyBroken);
            if (!xml.IsEmpty())
            {
                try
                {
                    var cleanedXml = xml.Replace("\n", " ").Replace("\r", " ").Replace("\t"," ").Trim();
                    // caters for ccNet v1.5 new xml format for CurrentMessage (messages/message)
                    var query = (from p in
                                     (from project in XDocument.Parse(cleanedXml)
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
                                            ProjectActivity = p.ProjectActivity
                                        });

                    if (_showOnlyBroken)
                    {
                        query = query.Where(p => p.IsBroken );
                    }

                    result.Projects = query.ToList();
                }
            
                catch (Exception e)
                {
                    _log.Error(e);
                }
            }

            return result;
        }

        public void ConfigUpdated(ConfigSettings newSettings)
        {
            SetLocalValuesFromConfig(newSettings);
        }

        void SetLocalValuesFromConfig(IConfigSettings newSettings)
        {
            _projectNameRegEx = new Regex(newSettings.ProjectNameRegEx);
            _categoryRegEx = new Regex(newSettings.CategoryRegEx);
            _showOnlyBroken = newSettings.ShowOnlyBroken;
            _viewName = newSettings.ViewName;
        }
    }
}