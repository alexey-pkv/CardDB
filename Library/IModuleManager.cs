namespace Library
{
	public interface IModuleManager
	{
		public void Initialize(IConfig config);
		public void Boot();
		public void Stop();
		
		public IModuleContainer Container();
	}
}