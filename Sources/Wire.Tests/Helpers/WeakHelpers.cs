using System;
namespace Wire.Tests
{
	public static class WeakHelpers
	{
		public static void ExecuteAndCollect(Action execute)
		{
			execute();

			// Forcing garbage collection
			GC.Collect();
			GC.WaitForPendingFinalizers();
			GC.Collect();
		}
	}
}
