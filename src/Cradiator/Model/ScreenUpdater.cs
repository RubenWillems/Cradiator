using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Cradiator.Audio;
using Cradiator.Views;

namespace Cradiator.Model
{
	public interface IScreenUpdater 
	{
		void Update();
	}

	public class ScreenUpdater : IScreenUpdater
	{
		readonly ICradiatorView _view;
		readonly DiscJockey _discJockey;
		readonly ICountdownTimer _countdownTimer;
		readonly IPollTimer _pollTimer;
		readonly BuildDataFetcher _fetcher;
		readonly BuildDataTransformer _transformer;
		readonly FetchExceptionHandler _fetchExceptionHandler;
		readonly BackgroundWorker _worker;

		public ScreenUpdater(ICradiatorView view, DiscJockey discJockey,
							 ICountdownTimer countdownTimer, IPollTimer pollTimer,
		                     BuildDataFetcher buildDataFetcher, BuildDataTransformer transformer,
		                     FetchExceptionHandler fetchExceptionHandler, BackgroundWorker worker)
		{
			_view = view;
			_discJockey = discJockey;
			_countdownTimer = countdownTimer;
			_pollTimer = pollTimer;
			_pollTimer.Tick = (sender, e) => Update();
			_fetcher = buildDataFetcher;
			_fetchExceptionHandler = fetchExceptionHandler;
			_transformer = transformer;

			_worker = worker;
			worker.DoWork += FetchData;
			worker.RunWorkerCompleted += DataFetched;
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
			    var projectData = xmlResults.SelectMany(xml => _transformer.Transform(xml));
				_view.DataContext = projectData;
				_discJockey.PlaySounds(projectData);
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