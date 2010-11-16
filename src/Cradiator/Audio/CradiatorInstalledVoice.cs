namespace Cradiator.Audio
{
	/// <summary>
	/// a simplification of InstalledVoice, to give us better, customised control over the same concept
	/// </summary>
	public class CradiatorInstalledVoice
	{
		public string Name { get; private set; }

		public CradiatorInstalledVoice(string name)
		{
			Name = name;
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			return ((CradiatorInstalledVoice)obj).Name == Name;		//good enough for us
		}

		public override int GetHashCode()
		{
			return Name.GetHashCode();
		}
	}
}