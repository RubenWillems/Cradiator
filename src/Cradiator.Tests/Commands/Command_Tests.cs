using System.Windows;
using Cradiator.Commands;
using Cradiator.Views;
using NUnit.Framework;
using Rhino.Mocks;

namespace Cradiator.Tests.Commands
{
	[TestFixture]
	public class Command_Tests
	{
		ICradiatorView _view;

		[SetUp]
		public void SetUp()
		{
			_view = MockRepository.GenerateMock<ICradiatorView>();
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

			Assert.That(command.CanExecute(null), Is.True);
			Assert.That(window.WindowStyle == WindowStyle.None);
			Assert.That(window.Topmost, Is.True);
			Assert.That(window.WindowState == WindowState.Maximized);
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

			Assert.That(window.WindowStyle, Is.EqualTo(WindowStyle.SingleBorderWindow));
			Assert.That(window.Topmost, Is.EqualTo(false));
			Assert.That(window.WindowState, Is.EqualTo(WindowState.Normal));
		}

		[Test]
		public void CanRefresh()
		{
			var command = new RefreshCommand(_view);
			command.Execute(null);

			Assert.That(command.CanExecute(null), Is.True);
			_view.AssertWasCalled(v=>v.UpdateScreen());
		}

		[Test]
		public void CanShowSettingsCommand()
		{
			var settingsWindow = MockRepository.GenerateMock<ISettingsWindow>();

			var command = new ShowSettingsCommand(_view, settingsWindow);
			command.Execute(null);

			Assert.That(command.CanExecute(null), Is.True);
			settingsWindow.AssertWasCalled(s => s.ShowDialog());
		}
	}
}