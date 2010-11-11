using System.Windows;
using Cradiator.Views;

namespace Cradiator.Model
{
	public interface ISkinLoader 
	{
		void Load(Skin newSkin);
	}

	public class SkinLoader : ISkinLoader
	{
		readonly ICradiatorView _view;
		readonly SkinResourceLoader _skinResourceLoader;

		public SkinLoader(ICradiatorView view, SkinResourceLoader skinResourceLoader)
		{
			_view = view;
			_skinResourceLoader = skinResourceLoader;
		}

		public void Load(Skin newSkin)
		{
			_view.Invoke(() =>
			{
				newSkin.Resource = _skinResourceLoader.LoadOrGet(newSkin);

			    var appResources = Application.Current.Resources.MergedDictionaries;

			    if (appResources.Contains(newSkin.Resource))
					appResources.Remove(newSkin.Resource);

			    appResources.Add(newSkin.Resource);
			});
		}
	}
}