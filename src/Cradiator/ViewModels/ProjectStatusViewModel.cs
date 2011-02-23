using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;

namespace Cradiator.ViewModels
{
    public class ProjectStatusViewModel : NotifyingClass
    {
        private string _name;
        private string _currentState;
        private string _CurrentMessage;
        private string _ProjectActivity;
        private string _serverName;
        private bool _isBroken;
        private bool _isSuccessful;
        private Visibility _serverNameVisible;
        private DateTime _lastBuildTime;

        [Obsolete("only used by xaml")]
        public ProjectStatusViewModel()
        {
        }


        public ProjectStatusViewModel(Model.ProjectStatus ps, Config.IViewSettings vs)
        {
            this.CurrentMessage = ps.CurrentMessage;
            this.CurrentState = ps.CurrentState;
            this.IsBroken = ps.IsBroken;
            this.IsSuccessful = ps.IsSuccessful;
            this.Name = ps.Name;
            this.ProjectActivity = ps.ProjectActivity.ToString();
            this.ServerName = ps.ServerName;
            this.LastBuildTime = ps.LastBuildTime;

            if (vs.ShowServerName)
            {
                this.ServerNameVisible = Visibility.Visible;
            }
            else
            {
                this.ServerNameVisible = Visibility.Collapsed;
            }
        }

        #region ProjectStatusProps

        public string Name
        {
            get { return _name; }
            set
            {
                if (_name == value) return;
                _name = value;
                Notify("Name");
            }
        }

        public string CurrentState
        {
            get { return _currentState; }
            set
            {
                if (_currentState == value) return;
                _currentState = value;
                Notify("CurrentState");
            }
        }

        public string CurrentMessage
        {
            get { return _CurrentMessage; }
            set
            {
                if (_CurrentMessage == value) return;
                _CurrentMessage = value;
                Notify("CurrentMessage");
            }
        }

        public string ProjectActivity
        {
            get { return _ProjectActivity; }
            set
            {
                if (_ProjectActivity == value) return;
                _ProjectActivity = value;
                Notify("ProjectActivity");
            }
        }

        public bool IsBroken
        {
            get { return _isBroken; }
            set
            {
                if (_isBroken == value) return;
                _isBroken = value;
                Notify("IsBroken");
            }
        }

        public bool IsSuccessful
        {
            get { return _isSuccessful; }
            set
            {
                if (_isSuccessful == value) return;
                _isSuccessful = value;
                Notify("IsSuccessful");
            }
        }


        public string ServerName
        {
            get { return _serverName; }

            set
            {
                if (_serverName == value) return;
                _serverName = value;
                Notify("ServerName");
            }
        }


        public DateTime LastBuildTime
        {
            get { return _lastBuildTime; }
            set
            {
                if (_lastBuildTime == value) return;
                _lastBuildTime = value;
                Notify("LastBuildTime");
            }
        }


        #endregion


        #region SettingProps

        public Visibility ServerNameVisible
        {
            get { return _serverNameVisible; }
            set
            {
                if (_serverNameVisible == value) return;
                _serverNameVisible = value;
                Notify("ServerNameVisible");
            }
        }

        #endregion
    }
}
