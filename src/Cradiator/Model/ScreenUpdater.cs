using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cradiator.Audio;
using Cradiator.Config;
using Cradiator.Views;
using log4net;

namespace Cradiator.Model
{
	public interface IScreenUpdater 
	{
		void Update();
	}

	public class ScreenUpdater : IScreenUpdater
	{
		static readonly ILog _log = LogManager.GetLogger(typeof(ScreenUpdater).Name);

		readonly ICradiatorView _view;
		readonly DiscJockey _discJockey;
		readonly ICountdownTimer _countdownTimer;
		readonly IPollTimer _pollTimer;
		private readonly IConfigSettings _configSettings;
		readonly BuildDataFetcher _fetcher;
		readonly BuildDataTransformer _transformer;
		readonly FetchExceptionHandler _fetchExceptionHandler;
		readonly BackgroundWorker _worker;

		public ScreenUpdater(ICradiatorView view, DiscJockey discJockey, ICountdownTimer countdownTimer, 
							 IPollTimer pollTimer, IConfigSettings configSettings, 
							 BuildDataFetcher buildDataFetcher, BuildDataTransformer transformer, 
							 FetchExceptionHandler fetchExceptionHandler, BackgroundWorker worker)
		{
			_view = view;
			_discJockey = discJockey;
			_countdownTimer = countdownTimer;
			_pollTimer = pollTimer;
			_configSettings = configSettings;
			_pollTimer.Tick = (sender, e) => PollTimeup();
			_fetcher = buildDataFetcher;
			_fetchExceptionHandler = fetchExceptionHandler;
			_transformer = transformer;

			_worker = worker;
			worker.DoWork += FetchData;
			worker.RunWorkerCompleted += DataFetched;
		}

	    private void PollTimeup()
		{
			_configSettings.RotateView();
			Update();
		}

		public void Update()
		{
			_countdownTimer.Stop();
			_pollTimer.Stop();
			_view.ShowProgress = true;

			_worker.RunWorkerAsync();
		}

		void FetchData(object sender, DoWorkEventArgs e)
		{
			try
			{
				var xmlResults = _fetcher.Fetch();
				e.Result = xmlResults;
			}
			catch (Exception exception)
			{
				_fetchExceptionHandler.Handle(exception);
			}
		}

		void DataFetched(object sender, RunWorkerCompletedEventArgs e)
		{
			try
			{
				var xmlResults = e.Result as IEnumerable<string>;
                ViewData projectData = new ViewData();
				if (xmlResults != null)
				{
                    var xml = string.Join("", xmlResults.ToArray());
                    projectData = _transformer.Transform(xml);
				}

				_view.DataContext = projectData;
                _discJockey.PlaySounds(projectData.Projects);
			}
			finally
			{
				_view.ShowProgress = false;

				_pollTimer.Start();
				_countdownTimer.Reset();
				_countdownTimer.Start();
			}
		}
	}
}