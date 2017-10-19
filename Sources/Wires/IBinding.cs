namespace Wires
{
	using System;

	public interface IBinding : IDisposable
	{
		bool IsAlive { get; }
	}
}