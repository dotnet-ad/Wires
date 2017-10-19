namespace Wire
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Reflection;

	/// <summary>
	/// Very basic helper for simple data binding.
	/// </summary>
	public abstract class Binding<TSource,TTarget, TSourceProperty, TTargetProperty> : IBinding
	{
		public Binding(TSource source, string sourceProperty, TTarget target, string targetProperty, IConverter<TSourceProperty, TTargetProperty> converter)
		{
			this.SourceReference = new WeakReference(source);
			this.TargetReference = new WeakReference(target);
			this.SourceProperty = sourceProperty;
			this.TargetProperty = targetProperty;
			var sourcePropertyInfo = source.GetType().GetRuntimeProperty(sourceProperty);
			var targetPropertyInfo = target.GetType().GetRuntimeProperty(targetProperty);
			this.Converter = converter;

			if (sourcePropertyInfo.PropertyType != typeof(TSourceProperty))
				throw new ArgumentException($"The given source property should be of type {typeof(TSourceProperty)}", nameof(sourceProperty));

			if (targetPropertyInfo.PropertyType != typeof(TTargetProperty))
				throw new ArgumentException($"The given target property should be of type {typeof(TTargetProperty)}", nameof(targetProperty));

			this.sourceGetter = sourcePropertyInfo.BuildGetExpression();
			this.sourceSetter = sourcePropertyInfo.BuildSetExpression();
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

		public WeakReference TargetReference { get; private set; }

		public WeakReference SourceReference { get; private set; }

		public IConverter<TSourceProperty,TTargetProperty> Converter { get; private set; }

		public TTarget Target
		{
			get
			{
				if(this.TargetReference.IsAlive)
					return (TTarget)this.TargetReference.Target;

				throw new InvalidOperationException($"{nameof(Target)} has been garbage collected and can't be used anymore");
			}
		}

		public TSource Source
		{
			get
			{
				if (this.SourceReference.IsAlive)
					return (TSource)this.SourceReference.Target;

				throw new InvalidOperationException($"{nameof(Source)} has been garbage collected and can't be used anymore");
			}
		}

		public bool IsAlive => TargetReference.IsAlive && SourceReference.IsAlive && !this.isDisposed;

		#endregion

		#region Updates

		public void UpdateSource()
		{
			if (IsAlive)
			{
				var sourceValue = (TSourceProperty)this.sourceGetter(SourceReference.Target);
				var targetValue = (TTargetProperty)this.targetGetter(TargetReference.Target);
				if (!object.Equals(sourceValue, targetValue))
				{
					var value = this.Converter.ConvertBack(targetValue);
					this.sourceSetter(SourceReference.Target, value);
					Debug.WriteLine($"[Bindings]({Source.GetType().Name}:{Source.GetHashCode()}) ~={{{targetValue}}}=> ({Target.GetType().Name}:{Target.GetHashCode()})");
				}
			}
		}

		public void UpdateTarget()
		{
			if (IsAlive)
			{
				var sourceValue = (TSourceProperty)this.sourceGetter(SourceReference.Target);
				var targetValue = (TTargetProperty)this.targetGetter(TargetReference.Target);
				if (!object.Equals(sourceValue, targetValue))
				{
					var value = this.Converter.Convert(sourceValue);
					this.targetSetter(TargetReference.Target, value);
					Debug.WriteLine($"[Bindings]({Source.GetType().Name}:{Source.GetHashCode()}) <={{{sourceValue}}}=~ ({Target.GetType().Name}:{Target.GetHashCode()})");
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
