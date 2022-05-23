using Library;


namespace CardDB.Modules
{
	public delegate void StopSignalReceived();
	
	
	public interface ISignalsModule : IModule 
	{
		public event StopSignalReceived OnStopSignal;
	}
}