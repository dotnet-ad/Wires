namespace Wire
{
	using System;

	public interface IBinding : IDisposable
	{
		WeakReference TargetReference { get; }

		WeakReference SourceReference { get; }

		string TargetProperty { get; }

		string SourceProperty { get; }

		bool IsAlive { get; }
	}
}