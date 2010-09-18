using Cradiator.App;
using Cradiator.Config;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Services
{
    [TestFixture]
    public class CruiseNinjaModule_Tests
    {
        [Test, Ignore("No longer valid because config.Load must be called earlier, reconsider test")]
        public void CanBoot()
        {
            var view = MockRepository.GenerateStub<ICradiatorView>();
            var configSettings = MockRepository.GenerateStub<IConfigSettings>();
            configSettings.ProjectNameRegEx = ".*";
            configSettings.CategoryRegEx = ".*";
            var boot = new Bootstrapper(configSettings, view);
            boot.CreateKernel();

            configSettings.ShouldHaveBeenCalled(c=>c.Load());
        }
    }
}