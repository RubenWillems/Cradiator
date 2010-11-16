using System.Windows;
using Cradiator.Commands;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;
using Shouldly;

namespace Cradiator.Tests.Commands
{
	[TestFixture]
	public class Command_Tests
	{
		ICradiatorView _view;

		[SetUp]
		public void SetUp()
		{
			_view = Create.Mock<ICradiatorView>();
		}

		[Test]
		[RequiresSTA]
		public void GoesFullscreen_IfNotAlready_Fullscreen()
		{
			var window = new Window
			{
				WindowStyle = WindowStyle.SingleBorderWindow,
				Topmost = false,
				WindowState = WindowState.Normal
			};

			_view.Expect(v => v.Window).Return(window).Repeat.Any();

			var command = new FullscreenCommand(_view);
			command.Execute(null);

		    command.CanExecute(null).ShouldBe(true);
			window.WindowStyle.ShouldBe(WindowStyle.None);
			window.Topmost.ShouldBe(true);
			window.WindowState.ShouldBe(WindowState.Maximized);
		}

		[Test]
		[RequiresSTA]
		public void GoesNotFullscreen_WhenAlready_FullScreen()
		{
			var window = new Window
			{
				WindowStyle = WindowStyle.None,
				Topmost = true,
				WindowState = WindowState.Maximized
			};

			_view.Expect(v => v.Window).Return(window).Repeat.Any();

			var command = new FullscreenCommand(_view);
			command.Execute(null);

			window.WindowStyle.ShouldBe(WindowStyle.SingleBorderWindow);
			window.Topmost.ShouldBe(false);
			window.WindowState.ShouldBe(WindowState.Normal);
		}

		[Test]
		public void CanRefresh()
		{
			var command = new RefreshCommand(_view);
			command.Execute(null);

			command.CanExecute(null).ShouldBe(true);
			_view.ShouldHaveBeenCalled(v=>v.UpdateScreen());
		}

		[Test]
		public void CanShowSettingsCommand()
		{
			var settingsWindow = MockRepository.GenerateMock<ISettingsWindow>();

			var command = new ShowSettingsCommand(_view, settingsWindow);
			command.Execute(null);

			command.CanExecute(null).ShouldBe(true);
            settingsWindow.ShouldHaveBeenCalled(s => s.ShowDialog());
		}
	}
}