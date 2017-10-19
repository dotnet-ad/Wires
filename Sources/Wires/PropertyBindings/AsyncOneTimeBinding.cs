
namespace Wires
{
	using System;
	using System.Diagnostics;
	using System.Threading.Tasks;
	using Transmute;

	public class AsyncOneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty> : PropertyBinding<TSource, TTarget, TSourceProperty, TTargetProperty>
		where TSource : class
		where TTarget : class
	{
		public AsyncOneTimeBinding(TSource source, Func<TSource, TSourceProperty> sourceGetter, TTarget target, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, Task<TTargetProperty>> asyncConverter, TTargetProperty loadingValue) : base(source, sourceGetter, null, target, targetGetter, targetSetter, null)
		{
			this.asyncConverter = asyncConverter;
			this.loadingValue = loadingValue;
			this.Update(); // Affect initial source value to target on binding
		}

		readonly TTargetProperty loadingValue;

		readonly IConverter<TSourceProperty, Task<TTargetProperty>> asyncConverter;

		public override async void Update()
		{
			TSource source;
			TTarget target;

			if (this.TryGet(out source, out target))
			{
				var initialValue = this.targetGetter(target);
				try
				{
					this.targetSetter(target, this.loadingValue);
					Debug.WriteLine($"[AsyncBindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{ ... (loading) ... }}=> ({target.GetType().Name}:{target.GetHashCode()})");
					var sourceValue = await asyncConverter.Convert(this.sourceGetter(source));
					this.targetSetter(target, sourceValue);
					Debug.WriteLine($"[AsyncBindings]({source.GetType().Name}:{source.GetHashCode()}) ~={{{sourceValue}}}=> ({target.GetType().Name}:{target.GetHashCode()})");
					this.Dispose(); // Dispose binding because we don't need it anymore since its one time.
				}
				catch
				{
					this.targetSetter(target, initialValue); // if update failed, then resets the value to its initial one
				}
				finally
				{
					this.Dispose(); // Dispose binding because we don't need it anymore since its one time.
				}
			}
		}
	}
}
