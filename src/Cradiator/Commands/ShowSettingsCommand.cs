using System;
using System.Windows.Input;
using Cradiator.Views;

namespace Cradiator.Commands
{	
	public class ShowSettingsCommand : ICommand
	{
		readonly ICradiatorView _view;
		readonly ISettingsWindow _settingsWindow;

		public ShowSettingsCommand(ICradiatorView view, ISettingsWindow settingsWindow)
		{
			_view = view;
			_settingsWindow = settingsWindow;
		}

		public bool CanExecute(object parameter)
		{
			return _view != null;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_settingsWindow.ShowDialog();
		}
	}
}