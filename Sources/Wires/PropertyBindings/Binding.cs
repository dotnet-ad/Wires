namespace Wires
{
	using System;
	using System.Diagnostics;
	using System.Reflection;

	/// <summary>
	/// A data binding from a source to a target.
	/// </summary>
	public abstract class Binding<TSource,TTarget, TSourceProperty, TTargetProperty> : WeakPair<TSource,TTarget>, IBinding 
		where TSource : class 
		where TTarget : class
	{
		#region Constructors

		public Binding(TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter) : base(source,target)
		{
			this.Converter = converter;

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

		protected bool isDisposed;

		#endregion

		#region Properties

		public IConverter<TSourceProperty,TTargetProperty> Converter { get; private set; }

		#endregion

		#region Updates


		/// <summary>
		/// Updates the source value from the target value.
		/// </summary>
		public virtual void UpdateSource()
		{
			TSource source;
			TTarget target;

			if (TryGet(out source,out target))
			{
				var targetValue = this.Converter.ConvertBack(this.targetGetter(target));
				this.sourceSetter(source, targetValue);
				Debug.WriteLine($"[Bindings]({source.GetType().Name}:{source.GetHashCode()}) <={{{targetValue}}}=~ ({target.GetType().Name}:{target.GetHashCode()})");
			}
		}

		/// <summary>
		/// Updates the target value from the source value.
		/// </summary>
		public virtual void UpdateTarget()
		{
			TSource source;
			TTarget target;

			if (TryGet(out source, out target))
			{
				var sourceValue = this.Converter.Convert(this.sourceGetter(source));
				this.targetSetter(target, sourceValue);
				Debug.WriteLine($"[Bindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{{sourceValue}}}=> ({target.GetType().Name}:{target.GetHashCode()})");
			}
		}

		#endregion
	}
}
