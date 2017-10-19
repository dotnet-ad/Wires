namespace Wires
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using Transmute;

	/// <summary>
	/// A binding that 
	/// </summary>
	public class AsyncOneWayBinding<TSource,TTarget,TSourceProperty, TTargetProperty,TSourceChangedEventArgs> : OneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, TSourceChangedEventArgs>
		where TSourceChangedEventArgs : EventArgs
		where TSource : class
		where TTarget : class
	{
		public AsyncOneWayBinding(TSource source, Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter, string sourceUpdateEvent, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, Task<TTargetProperty>> asyncConverter, TTargetProperty loadingValue, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null) : base(source, sourceGetter, sourceSetter, sourceUpdateEvent, target, targetGetter, targetSetter, null, sourceEventFilter)
		{
			this.asyncConverter = asyncConverter;
			this.loadingValue = loadingValue;

			this.Update();
		}

		readonly TTargetProperty loadingValue;

		readonly IConverter<TSourceProperty, Task<TTargetProperty>> asyncConverter;

		public override async void Update()
		{
			if (this.asyncConverter != null)
			{

				TSource source;
				TTarget target;

				if (this.TryGet(out source, out target))
				{
					try
					{
						this.targetSetter(target, this.loadingValue);
						Debug.WriteLine($"[AsyncBindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{ ... (loading) ... }}=> ({target.GetType().Name}:{target.GetHashCode()})");
						var sourceValue = await asyncConverter.Convert(this.sourceGetter(source));
						this.targetSetter(target, sourceValue);
						Debug.WriteLine($"[AsyncBindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{{sourceValue}}}=> ({target.GetType().Name}:{target.GetHashCode()})");
					}
					catch (Exception e)
					{
						Debug.WriteLine($"[AsyncBindings] Failed to update value : {e}");
					}
				}
			}
		}
	}
}
