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
		private Binding(TSource source, string sourceProperty, TTarget target, IConverter<TSourceProperty, TTargetProperty> converter)
		{
			this.Converter = converter;

			this.SourceReference = new WeakReference<TSource>(source);
			this.TargetReference = new WeakReference<TTarget>(target);

			// Source property
			this.SourceProperty = sourceProperty;
			var sourcePropertyInfo = source.GetType().GetRuntimeProperty(sourceProperty);

			if (sourcePropertyInfo.PropertyType != typeof(TSourceProperty))
				throw new ArgumentException($"The given source property should be of type {typeof(TSourceProperty)}", nameof(sourceProperty));

			this.sourceGetter = sourcePropertyInfo.BuildGetExpression();
			this.sourceSetter = sourcePropertyInfo.BuildSetExpression();
		}

		public Binding(TSource source, string sourceProperty, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter) : this(source, sourceProperty, target, converter)
		{
			this.targetGetter = (i => targetGetter((TTarget)i)) ;
			this.targetSetter = ((i, v) => targetSetter((TTarget)i, (TTargetProperty)v));
		}

		public Binding(TSource source, string sourceProperty, TTarget target, string targetProperty, IConverter<TSourceProperty, TTargetProperty> converter) : this(source,sourceProperty,target, converter)
		{
			// Target property
			this.TargetProperty = targetProperty;
			var targetPropertyInfo = target.GetType().GetRuntimeProperty(targetProperty);

			if (targetPropertyInfo.PropertyType != typeof(TTargetProperty))
				throw new ArgumentException($"The given target property should be of type {typeof(TTargetProperty)}", nameof(targetProperty));

			this.targetGetter = targetPropertyInfo.BuildGetExpression();
			this.targetSetter = targetPropertyInfo.BuildSetExpression();
		}

		#region Fields

		readonly Action<object, object> targetSetter;

		readonly Func<object, object> targetGetter;

		readonly Action<object, object> sourceSetter;

		readonly Func<object, object> sourceGetter;

		bool isDisposed;

		#endregion

		#region Properties

		public string TargetProperty { get; private set; }

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
				var sourceValue = (TSourceProperty)this.sourceGetter(source);
				var targetValue = this.Converter.ConvertBack((TTargetProperty)this.targetGetter(target));
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
				var sourceValue = this.Converter.Convert((TSourceProperty)this.sourceGetter(source));
				var targetValue = (TTargetProperty)this.targetGetter(target);
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
