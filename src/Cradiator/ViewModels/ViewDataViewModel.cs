using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;
using System.Windows;
using Cradiator.Model;
using Cradiator.Config;

namespace Cradiator.ViewModels
{
    /// <summary>
    /// Master data over a certain view
    /// so you could show some statistics or whatever besides just the project names and their states
    /// </summary>
    public class ViewDataViewModel : NotifyingClass
    {
        private List<ProjectStatusViewModel> _projects = new List<ProjectStatusViewModel>();
        private decimal _okPercentage;
        private string _AmountHeader;
        private int _AmountTotal;
        private int _AmountOK;
        private int _AmountNotOK;
        private bool _ShowOnlyBroken;
        private Visibility _ShowProjects;
        private Visibility _ShowAllOK;

        private string _viewName;


        [Obsolete("only used by XAML")]
        public ViewDataViewModel()
        {
        }


        public ViewDataViewModel(IViewSettings vs, IEnumerable<ProjectStatus> projects)
        {

            _viewName = vs.ViewName;
            _ShowOnlyBroken = vs.ShowOnlyBroken;

            List<ProjectStatusViewModel> dummy = new List<ProjectStatusViewModel>();

            foreach (Model.ProjectStatus p in projects)
            {
                dummy.Add(new ProjectStatusViewModel(p, vs));
            }

            this.Projects = dummy;
        }

        /// <summary>
        /// The name of the view, taken from the config
        /// only of importance when ShowOnlyBroken is true and all projects are ok
        /// in that case the smiley is shown, so you know what view is ok
        /// </summary>
        public string ViewName
        {
            get { return _viewName; }
            set
            {
                if (_viewName == value) return;
                _viewName = value;
                Notify("ViewName");
            }
        }


        /// <summary>
        /// Just a list and not a bindinglist, because we just overwrite the entire list,
        /// and are not updating individual projects.
        /// Whenever the list of projects is changed by setting a new value,
        /// the other properties will also be recalculated.
        /// </summary>
        public List<ProjectStatusViewModel> Projects
        {
            get 
            { 
                if (ShowOnlyBroken) return _projects.Where(p => p.IsBroken).ToList();
                return _projects;
            
            }
            set
            {
                _projects = value;
                Notify("Projects");
                CalculateProperties();
            }
        }


        public decimal OKPercentage
        {
            get { return _okPercentage; }
            set
            {
                if (_okPercentage == value) return;
                _okPercentage = value;
                Notify("OKPercentage");
            }
        }

        public string AmountHeader
        {
            get { return _AmountHeader; }
            set
            {
                if (_AmountHeader == value) return;
                _AmountHeader = value;
                Notify("AmountHeader");
            }
        }


        public int AmountTotal
        {
            get { return _AmountTotal; }
            set
            {
                if (_AmountTotal == value) return;
                _AmountTotal = value;
                Notify("AmountTotal");
            }
        }



        public int AmountOK
        {
            get { return _AmountOK; }
            set
            {
                if (_AmountOK == value) return;
                _AmountOK = value;
                Notify("AmountOK");
            }
        }

        public int AmountNotOK
        {
            get { return _AmountNotOK; }
            set
            {
                if (_AmountNotOK == value) return;
                _AmountNotOK = value;
                Notify("AmountNotOK");
            }
        }

        /// <summary>
        /// Taken from the config and needed for determining to show the smiley or not
        /// </summary>
        public bool ShowOnlyBroken
        {
            get { return _ShowOnlyBroken; }
            set
            {
                if (_ShowOnlyBroken == value) return;
                _ShowOnlyBroken = value;
                Notify("ShowOnlyBroken");
            }
        }

        /// <summary>
        /// Show the projects viewbox
        /// </summary>
        public Visibility ShowProjects
        {
            get { return _ShowProjects; }
            set
            {
                if (_ShowProjects == value) return;
                _ShowProjects = value;
                Notify("ShowProjects");
            }
        }


        /// <summary>
        /// Show the AllOK viewbox aka smiley
        /// </summary>
        public Visibility ShowAllOK
        {
            get { return _ShowAllOK; }
            set
            {
                if (_ShowAllOK == value) return;
                _ShowAllOK = value;
                Notify("ShowAllOK");
            }
        }


        private void CalculateProperties()
        {
            if (this._projects == null) this._projects = new List<ProjectStatusViewModel>();

            AmountTotal = this._projects.Count;
            AmountOK = this._projects.Count(x => x.IsSuccessful);
            AmountNotOK = this._projects.Count - AmountOK;

            if (this.AmountTotal == 0)
            {
                OKPercentage = 100;
            }
            else
            {
                OKPercentage = AmountOK / AmountTotal;
                OKPercentage *= 100;
                OKPercentage = decimal.Round(OKPercentage, 4);
            }


            //when showonly broken is set to true, and everything is ok, show the OK screen
            if (ShowOnlyBroken && OKPercentage == 100)
            {
                ShowAllOK = Visibility.Visible;
                ShowProjects = Visibility.Collapsed;

            }
            else
            {
                ShowAllOK = Visibility.Collapsed;
                ShowProjects = Visibility.Visible;
            }

            AmountHeader = string.Format("Project Count : {0} ", this.AmountTotal);
        }


    }
}
