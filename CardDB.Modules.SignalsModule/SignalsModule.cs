using System;
using Library;


namespace CardDB.Modules.SignalsModule
{
	public class SignalsModule : AbstractModule, ISignalsModule
	{
		public override string Name => "SystemSignals";
		
		
		private void Stop()
		{
			lock (this)
			{
				OnStopSignal?.Invoke();
			}
			
			ApplicationStopLock.Wait();
		}
		

		public override void Init()
		{
			Console.CancelKeyPress += (sender, args) =>
			{
				if (LogRepository.IsSetup && !ApplicationStopLock.IsStopping())
					Log.Info("Got Ctrl-C event. Stopping...");

				Stop();
			};
			
			// Should be called on the SIGTERM (or similar) signal, but also called
			// when the main thread throws and exception.
			// Probably it's also some kind of a signal. 
			AppDomain.CurrentDomain.ProcessExit += (sender, args) =>
			{
				if (LogRepository.IsSetup && !ApplicationStopLock.IsStopping())
					Log.Info("Got Fatal Error or stop signal. Stopping...");
				
				Stop();
			};
		}
		
		
		public event StopSignalReceived OnStopSignal;
	}
}