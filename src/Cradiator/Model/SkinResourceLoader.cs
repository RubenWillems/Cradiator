using System.Collections.Generic;
using System.Windows;

namespace Cradiator.Model
{
	public class SkinResourceLoader
	{
		readonly IDictionary<string, ResourceDictionary> _resource = new Dictionary<string, ResourceDictionary>();

		public ResourceDictionary LoadOrGet(Skin skin)
		{
			if (!_resource.ContainsKey(skin.Name))
				_resource[skin.Name] = Application.LoadComponent(skin.ToUri) as ResourceDictionary;

			return _resource[skin.Name];
		}
	}
}