using System.ComponentModel;
using System.Windows;
using Cradiator.Audio;
using Cradiator.Config;

namespace Cradiator.Views
{
	public interface ISettingsWindow
	{
		bool? ShowDialog();
	}

	public partial class SettingsWindow : ISettingsWindow
	{
		readonly IConfigSettings _configSettings;
		readonly ICradiatorView _view;
		readonly ISpeechSynthesizer _speechSynthesizer;

		public SettingsWindow(IConfigSettings configSettings, ICradiatorView view, ISpeechSynthesizer speechSynthesizer)
		{
			_configSettings = configSettings;
			_view = view;
			_speechSynthesizer = speechSynthesizer;

			InitializeComponent();
			SetBindings();

			_view.Closing += ((sender, e) => Close());
			_view.Activated += ((sender, e) => Owner = view.Window);
			Closing += SettingsWindow_Closing;
		}

		void SettingsWindow_Closing(object sender, CancelEventArgs e)
		{
			e.Cancel = true;
			Hide();
		}

		void Save_Click(object sender, RoutedEventArgs e)
		{
			_configSettings.Save();
			Hide();
		}

		void Cancel_Click(object sender, RoutedEventArgs e)
		{
			_configSettings.Load();	// To 'Cancel' (ignoring changes), we simply reload from config
			Hide();
		}

		void SetBindings()
		{
			this.DataContext = _configSettings;
			comboInstalledVoices.ItemsSource = _speechSynthesizer.GetInstalledVoices();
		}
	}
}