using System;
namespace Wires
{
	public class WeakPair<TSource,TTarget> : IDisposable
		where TSource : class
		where TTarget : class
	{
		public WeakPair(TSource source, TTarget target)
		{
			this.SourceReference = new WeakReference<TSource>(source);
			this.TargetReference = new WeakReference<TTarget>(target);
		}

		public WeakReference<TTarget> TargetReference { get; private set; }

		public WeakReference<TSource> SourceReference { get; private set; }

		public bool IsDisposed { get; protected set; }

		public virtual void Dispose()
		{
			this.IsDisposed = true;
		}

		public bool TryGetSource(out TSource source)
		{
			source = default(TSource);
			return !this.IsDisposed && this.SourceReference.TryGetTarget(out source);
		}

		public bool TryGetTarget(out TTarget target)
		{
			target = default(TTarget);
			return !this.IsDisposed && this.TargetReference.TryGetTarget(out target);
		}

		public bool TryGet(out TSource source, out TTarget target)
		{
			source = default(TSource);
			target = default(TTarget);
			return !this.IsDisposed && this.SourceReference.TryGetTarget(out source) && this.TargetReference.TryGetTarget(out target);
		}

		public void Get(out TSource source, out TTarget target)
		{
			source = default(TSource);
			target = default(TTarget);
			if (!this.TryGet(out source, out target))
			{
				throw new InvalidOperationException("Binding is not alive anymore");
			}
		}

		public TSource Source
		{
			get
			{
				TSource source;
				if (this.TryGetSource(out source))
				{
					return source;
				}
				throw new InvalidOperationException("Binding is not alive anymore");
			}
		}

		public TTarget Target
		{
			get
			{
				TTarget target;
				if (this.TryGetTarget(out target))
				{
					return target;
				}
				throw new InvalidOperationException("Binding is not alive anymore");
			}
		}
	}
}
