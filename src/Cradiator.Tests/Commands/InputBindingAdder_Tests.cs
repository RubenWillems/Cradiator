using System.Windows.Input;
using Cradiator.Commands;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Commands
{
    [TestFixture]
    public class InputBindingAdder_Tests
    {
        ICradiatorView _view;
        ISettingsWindow _settingsWindow;

        [SetUp]
        public void SetUp()
        {
            _view = MockRepository.GenerateMock<ICradiatorView>();
            _settingsWindow = MockRepository.GenerateMock<ISettingsWindow>();
        }

        [Test]
        public void CanAddBindings()
        {
            var bindingAdder = new InputBindingAdder(_view,
                                                     new CommandContainer(new FullscreenCommand(_view),
                                                                          new RefreshCommand(_view),
                                                                          new ShowSettingsCommand(_view, _settingsWindow)));

            bindingAdder.AddBindings();

            _view.AssertWasCalled(v=>v.AddWindowBinding(Arg<InputBinding>.Is.Anything), v=>v.Repeat.AtLeastOnce());
        }
    }
}