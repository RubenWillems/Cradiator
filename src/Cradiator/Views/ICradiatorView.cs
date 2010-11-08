using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace Cradiator.Views
{
	public interface ICradiatorView
	{
		void ShowMessage(string message);
		object DataContext { set; }
		void UpdateCountdownTimer(TimeSpan timeRemaining);
		void Invoke(Action action);
		void ShowCountdown(bool show);
		Window Window{ get;}
		CradiatorPresenter Presenter { set; }
		bool ShowProgress { set; }
		void UpdateScreen();
		void AddWindowBinding(InputBinding binding);
		void AddSettingsLinkBinding(InputBinding binding);
		event CancelEventHandler Closing;
		event EventHandler Activated;
		event EventHandler ScreenUpdating;
	}
}