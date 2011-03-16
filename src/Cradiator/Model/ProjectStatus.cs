using Cradiator.Extensions;
using System;

namespace Cradiator.Model
{
	public class ProjectStatus
	{
		public const string BUILDING = "building";
		public const string SUCCESS = "success";
		public const string NORMAL = "normal";
		public const string FAILURE = "failure";
		public const string EXCEPTION = "exception";
		public const string ERROR = "error";
        public const string UNKNOWN = "unknown";

		public string Name { get;  private set; } 
		public string CurrentMessage { get; set; }
		public ProjectActivity ProjectActivity { get; set; }
		public string LastBuildStatus { get; set; }
        public string ServerName { get; set; }
        public DateTime LastBuildTime { get; set; }

        [Obsolete("Constructor for XAML")]
        public ProjectStatus() { Name = "designer name"; }


		public ProjectStatus(string name)
		{
			Name = name;
		}

		public string CurrentState
		{
			get { return (ProjectActivity == ProjectActivity.Building) ? BUILDING : LastBuildStatus; }
		}

		public bool IsBroken
		{
			get 
			{ 
				return LastBuildStatus.EqualsIgnoreCase(FAILURE) || 
					   LastBuildStatus.EqualsIgnoreCase(EXCEPTION) || 
					   LastBuildStatus.EqualsIgnoreCase(ERROR); 
			}
		}

		public bool IsSuccessful
		{
			get 
			{ 
				return LastBuildStatus.EqualsIgnoreCase(SUCCESS) || 
					   LastBuildStatus.EqualsIgnoreCase(NORMAL) ||
                       LastBuildStatus.EqualsIgnoreCase(UNKNOWN) ; // CCNET unknown is when is project has not build yet.
			}
		}

		public override bool Equals(object obj)
		{
			return ((ProjectStatus)obj).Name == Name;	// simple, but all that is needed
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}