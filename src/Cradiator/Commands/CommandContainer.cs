using System.Windows.Input;

namespace Cradiator.Commands
{
	public class CommandContainer
	{
		readonly ICommand _fullscreenCommand;
		readonly ICommand _refreshCommand;
		readonly ICommand _showSettingsCommand;

		// ReSharper disable SuggestBaseTypeForParameter
		public CommandContainer(
			FullscreenCommand fullScreenCommand, 
			RefreshCommand refreshCommand, 
			ShowSettingsCommand showSettingsCommand)
		{
			_fullscreenCommand = fullScreenCommand;
			_refreshCommand = refreshCommand;
			_showSettingsCommand = showSettingsCommand;
		}
		// ReSharper restore SuggestBaseTypeForParameter
        
		public ICommand FullscreenCommand
		{
			get { return _fullscreenCommand; }
		}

		public ICommand ShowSettingsCommand
		{
			get { return _showSettingsCommand; }
		}

		public ICommand RefreshCommand
		{
			get { return _refreshCommand; }
		}
	}
}