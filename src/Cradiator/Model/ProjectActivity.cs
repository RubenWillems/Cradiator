namespace Cradiator.Model
{
	/// <summary>
	/// Enumeration of the possible activities of a project under continuous
	/// integration by CruiseControl.NET.
	/// Class partly copied from CCTrayLib project (for use in Cradiator)
	/// </summary>
	public class ProjectActivity
	{
		private readonly string _activityType;

		public ProjectActivity(string type)
		{
			_activityType = type;
		}

		public override bool Equals(object obj)
		{
			var other = obj as ProjectActivity;
			return other != null && other.ToString() == ToString();
		}

		public override int GetHashCode()
		{
			return base.GetHashCode();
		}

		public override string ToString()
		{
			return _activityType;
		}

		public static bool operator == (ProjectActivity left, ProjectActivity right) 
		{
			return Equals(left, right);
		}

		public static bool operator != (ProjectActivity left, ProjectActivity right) 
		{
			return !(left == right);
		}

		public static readonly ProjectActivity Building = new ProjectActivity("Building");
	}
}