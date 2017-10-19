using System.Collections.Generic;
namespace Wires
{
	using System;
	using System.Diagnostics;
	using Transmute;

	/// <summary>
	/// A data binding from a source to a target.
	/// </summary>
	public abstract class PropertyBinding<TSource,TTarget, TSourceProperty, TTargetProperty> : Binding<TSource,TTarget>
		where TSource : class 
		where TTarget : class
	{
		#region Constructors

		public PropertyBinding(TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter) : base(source,target)
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
		/// Updates the target value from the source value.
		/// </summary>
		public virtual void Update()
		{
			TSource source;
			TTarget target;

			if (TryGet(out source, out target))
			{
				var targetValye = this.targetGetter(target);
				var sourceValue = this.Converter.Convert(this.sourceGetter(source));
				if(!EqualityComparer<TTargetProperty>.Default.Equals(sourceValue, targetValye))
				{
					this.targetSetter(target, sourceValue);
					Debug.WriteLine($"[Bindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{{sourceValue}}}=> ({target.GetType().Name}:{target.GetHashCode()})");
				}
			}
		}

		#endregion
	}
}
