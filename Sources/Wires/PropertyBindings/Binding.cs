namespace Wires
{
	using System;
	using System.Diagnostics;
	using System.Reflection;

	/// <summary>
	/// Very basic helper for simple data binding.
	/// </summary>
	public abstract class Binding<TSource,TTarget, TSourceProperty, TTargetProperty> : IBinding 
		where TSource : class 
		where TTarget : class
	{
		#region Constructors

		public Binding(TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter) 
		{
			this.Converter = converter;

			this.SourceReference = new WeakReference<TSource>(source);
			this.TargetReference = new WeakReference<TTarget>(target);

			this.sourceGetter = sourceGetter;
			this.sourceSetter = sourceSetter;
			this.targetGetter = targetGetter;
			this.targetSetter = targetSetter;
		}

		#endregion

		#region Fields

		protected Action<TTarget, TTargetProperty> targetSetter;

		protected Func<TTarget, TTargetProperty> targetGetter;

		protected readonly Action<TSource, TSourceProperty> sourceSetter;

		protected readonly Func<TSource, TSourceProperty> sourceGetter;

		bool isDisposed;

		#endregion

		#region Properties

		public string SourceProperty { get; private set; }

		public WeakReference<TTarget> TargetReference { get; private set; }

		public WeakReference<TSource> SourceReference { get; private set; }

		public IConverter<TSourceProperty,TTargetProperty> Converter { get; private set; }

		public bool IsAlive
		{
			get
			{
				TSource source;
				TTarget target;

				return this.SourceReference.TryGetTarget(out source) && this.TargetReference.TryGetTarget(out target) && !this.isDisposed;
			}
		}

		#endregion

		#region Updates

		public void UpdateSource()
		{
			TSource source;
			TTarget target;

			if (this.SourceReference.TryGetTarget(out source) && this.TargetReference.TryGetTarget(out target))
			{
				var sourceValue = this.sourceGetter(source);
				var targetValue = this.Converter.ConvertBack(this.targetGetter(target));
				if (!object.Equals(sourceValue, targetValue))
				{
					this.sourceSetter(source, targetValue);
					Debug.WriteLine($"[Bindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{{targetValue}}}=> ({target.GetType().Name}:{target.GetHashCode()})");
				}
			}
		}

		public void UpdateTarget()
		{
			TSource source;
			TTarget target;

			if (this.SourceReference.TryGetTarget(out source) && this.TargetReference.TryGetTarget(out target))
			{
				var sourceValue = this.Converter.Convert(this.sourceGetter(source));
				var targetValue = this.targetGetter(target);
				if (!object.Equals(sourceValue, targetValue))
				{
					this.targetSetter(target, sourceValue);
					Debug.WriteLine($"[Bindings]({source.GetType().Name}:{source.GetHashCode()}) <={{{sourceValue}}}=~ ({target.GetType().Name}:{target.GetHashCode()})");
				}
			}
		}

		public virtual void Dispose() 
		{
			this.isDisposed = true;
		}

		#endregion
	}
}
