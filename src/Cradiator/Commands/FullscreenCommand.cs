using System;
using System.Windows;
using System.Windows.Input;
using Cradiator.Views;

namespace Cradiator.Commands
{
	public class FullscreenCommand : ICommand
	{
		readonly ICradiatorView _view;

		public FullscreenCommand(ICradiatorView view)
		{
			_view = view;
		}

		public bool CanExecute(object parameter)
		{
			return _view != null && _view.Window != null;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			if (_view.Window.WindowStyle != WindowStyle.None)
			{
				_view.Window.WindowStyle = WindowStyle.None;
				_view.Window.Topmost = true;
				_view.Window.WindowState = WindowState.Maximized;
			}
			else
			{
				_view.Window.WindowStyle = WindowStyle.SingleBorderWindow;
				_view.Window.Topmost = false;
				_view.Window.WindowState = WindowState.Normal;
			}
		}
	}
}
