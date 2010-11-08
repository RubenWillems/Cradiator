using System;
using System.Windows;

namespace Cradiator.Model
{
	public class Skin
	{
		readonly string _name;

		public Skin(string skinName)
		{
			_name = skinName;
		}

		public string Name
		{
			get { return _name; }
		}

		public ResourceDictionary Resource { get; set; }

		public Uri ToUri
		{
			get
			{
				var skinPath = string.Format("./Skins/{0}Skin.xaml", Name);
				return new Uri(skinPath, UriKind.Relative);
			}
		}
	}
}