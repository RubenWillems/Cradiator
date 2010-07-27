using System.Reflection;
using System.Windows;
using Cradiator.App;
using Cradiator.Config;
using Cradiator.Model;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Model
{
	[TestFixture]
	public class SkinResourceController_Tests
	{
		[Test]
		public void CanGet_AlreadyLoaded_Resource()
		{
			var boot = new Bootstrapper(MockRepository.GenerateStub<IConfigSettings>(),
			                            MockRepository.GenerateStub<ICradiatorView>());
			boot.CreateKernel();

			var skin1 = new Skin("Stack"); // these tests are reliant on xaml file names in the main assembly (Skin folder)
			var skin2 = new Skin("Grid");
			var skin3 = new Skin("StackPhoto");

			Application.ResourceAssembly = Assembly.GetAssembly(typeof (Skin));

			var skinResourceLoader = new SkinResourceLoader();

			var resourceSkin1 = skinResourceLoader.LoadOrGet(skin1);
			var resourceSkin2 = skinResourceLoader.LoadOrGet(skin2);
			var resourceSkin3 = skinResourceLoader.LoadOrGet(skin3);

			Assert.That(skinResourceLoader.LoadOrGet(skin3), Is.SameAs(resourceSkin3));
			Assert.That(skinResourceLoader.LoadOrGet(skin2), Is.SameAs(resourceSkin2));
			Assert.That(skinResourceLoader.LoadOrGet(skin1), Is.SameAs(resourceSkin1));
		}
	}
}