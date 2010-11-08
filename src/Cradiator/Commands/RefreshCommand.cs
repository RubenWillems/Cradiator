using System;
using System.Windows.Input;
using Cradiator.Views;

namespace Cradiator.Commands
{
	public class RefreshCommand : ICommand
	{
		readonly ICradiatorView _view;

		public RefreshCommand(ICradiatorView view)
		{
			_view = view;
		}

		public bool CanExecute(object parameter)
		{
			return _view != null;
		}

		public event EventHandler CanExecuteChanged;

		public void Execute(object parameter)
		{
			_view.UpdateScreen();
		}
	}
}
