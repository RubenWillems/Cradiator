using System;
using Cradiator.Views;
using log4net;

namespace Cradiator.Model
{
	public class FetchExceptionHandler
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(FetchExceptionHandler).Name);

		readonly ICradiatorView _view;

		public FetchExceptionHandler(ICradiatorView view)
		{
			_view = view;
		}

		public void Handle(Exception fetchException)
		{
			_log.Error(fetchException.Message, fetchException);
			_view.DataContext = null;

			try
			{
				_view.Invoke(() => _view.ShowMessage("Connection problem"));
			}
			catch (Exception exception)
			{	// this really shouldn't happen but we're getting an inexplicable Win32Exception
				_log.Error(exception.Message, exception);
			}
		}
	}
}