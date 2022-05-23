using CardDB.Modules;
using Library;

namespace CardDB.Modules.TestModule
{
	public class TestModule : AbstractModule, ITestModule
	{
		public override string Name => "TestModule";
		public string GetTestValue() => Name;
	}
}