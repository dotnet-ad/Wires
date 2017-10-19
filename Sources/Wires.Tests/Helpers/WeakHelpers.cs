using System;
namespace Wires.Tests
{
	public static class WeakHelpers
	{
		public static void ExecuteAndCollect(Action execute)
		{
			execute();

			Bindings.Purge();

			// Forcing garbage collection
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}
}
