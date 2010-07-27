using System.Windows.Input;
using Cradiator.Views;

namespace Cradiator.Commands
{
	public class InputBindingAdder
	{
		readonly ICradiatorView _view;
		readonly CommandContainer _commandContainer;

		public InputBindingAdder(ICradiatorView view, CommandContainer commandContainer)
		{
			_view = view;
			_commandContainer = commandContainer;
		}

		public void AddBindings()
		{
			_view.AddWindowBinding(new KeyBinding(_commandContainer.RefreshCommand, Key.F5, ModifierKeys.None));
			_view.AddWindowBinding(new KeyBinding(_commandContainer.FullscreenCommand, Key.F11, ModifierKeys.None));

			_view.AddWindowBinding(new KeyBinding(_commandContainer.ShowSettingsCommand, Key.F12, ModifierKeys.None));
			_view.AddSettingsLinkBinding(new MouseBinding(_commandContainer.ShowSettingsCommand,
										 new MouseGesture(MouseAction.LeftClick)));

			_view.AddWindowBinding(new MouseBinding(_commandContainer.FullscreenCommand, 
								   new MouseGesture(MouseAction.LeftDoubleClick)));
		}
	}
}