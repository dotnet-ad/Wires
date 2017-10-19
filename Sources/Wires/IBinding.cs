namespace Wires
{
	using System;

	public interface IBinding : IDisposable
	{
		bool IsAlive { get; }

		bool TryGetSourceAndTarget(out object source, out object target);
	}
}