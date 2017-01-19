using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Wires
{
	/// <summary>
	/// A binder allows the creation of one or more bindings from a source to a target.
	/// </summary>
	public class Binder<TSource, TTarget> : WeakPair<TSource,TTarget>, IBinding
		where TSource : class
		where TTarget : class
	{
		public Binder(TSource source, TTarget target) : base(source,target)
		{
		}

		public bool IsAlive => !this.IsDisposed && this.bindings.Any(b => b.IsAlive);

		private List<IBinding> bindings = new List<IBinding>();

		private Binder<TSource, TTarget> Add(IBinding binding)
		{
			this.bindings.Add(binding);
			return this;
		}

		public Binder<TSource, TNewTarget> As<TNewTarget>()
			where TNewTarget : class
		{
			var cast = new Binder<TSource, TNewTarget>(Source, (TNewTarget)(object)Target);
			this.Add(cast);
			return cast;
		}

		#region OneTime

		private Binder<TSource, TTarget> OneTime<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();
			var targetAccessors = targetProperty.BuildAccessors();

			return Add(new OneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceAccessors.Item1, Target, targetAccessors.Item1, targetAccessors.Item2, converter));
		}

		private Binder<TSource, TTarget> OneTime<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();

			return Add(new OneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceAccessors.Item1, Target, targetGetter, targetSetter, converter));
		}

		#endregion

		#region Async bindings

		private Binder<TSource, TTarget> OneTimeAsync<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, Task<TTargetProperty>> converter, TTargetProperty loading = default(TTargetProperty))
		{
			var sourceAccessors = sourceProperty.BuildAccessors();
			var targetAccessors = targetProperty.BuildAccessors();



			return Add(new AsyncOneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceAccessors.Item1, Target, targetAccessors.Item1, targetAccessors.Item2, converter, loading));
		}

		private Binder<TSource, TTarget> OneTimeAsync<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, Task<TTargetProperty>> converter, TTargetProperty loading = default(TTargetProperty))
		{
			var sourceAccessors = sourceProperty.BuildAccessors();

			return Add(new AsyncOneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceAccessors.Item1, Target, targetGetter, targetSetter, converter, loading));
		}

		public Binder<TSource, TTarget> PropertyAsync<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, Task<TTargetProperty>> converter, TTargetProperty loading = default(TTargetProperty))
		{
			if (Source is INotifyPropertyChanged)
			{
				var sourceAccessors = sourceProperty.BuildAccessors();
				return Add(new AsyncOneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs>(Source, sourceAccessors.Item1, sourceAccessors.Item2, nameof(INotifyPropertyChanged.PropertyChanged), Target, targetGetter, targetSetter, converter, loading, (PropertyChangedEventArgs arg) => (arg.PropertyName == sourceAccessors.Item3)));
			}

			return this.OneTimeAsync(sourceProperty, targetGetter, targetSetter, converter, loading);
		}

		public Binder<TSource, TTarget> PropertyAsync<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, Task<TTargetProperty>> converter, TTargetProperty loading = default(TTargetProperty))
		{
			var targetAccessors = targetProperty.BuildAccessors();
			return this.PropertyAsync(sourceProperty, targetAccessors.Item1, targetAccessors.Item2, converter, loading);
		}

		#endregion

		/// <summary>
		/// Binds the property to a source and creates a OneWay binding if the source is a INotifyPropertyChanged implementation.
		/// </summary>
		/// <param name="sourceProperty">Source property.</param>
		/// <param name="targetGetter">Target getter.</param>
		/// <param name="targetSetter">Target setter.</param>
		/// <param name="converter">Converter.</param>
		/// <typeparam name="TSourceProperty">The 1st type parameter.</typeparam>
		/// <typeparam name="TTargetProperty">The 2nd type parameter.</typeparam>
		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			if (Source is INotifyPropertyChanged)
			{
				converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();
				var sourceAccessors = sourceProperty.BuildAccessors();
				return Add(new OneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs>(Source, sourceAccessors.Item1, sourceAccessors.Item2, nameof(INotifyPropertyChanged.PropertyChanged), Target, targetGetter, targetSetter, converter, (PropertyChangedEventArgs arg) => (arg.PropertyName == sourceAccessors.Item3)));
			}

			return this.OneTime(sourceProperty, targetGetter, targetSetter, converter);
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			var targetAccessors = targetProperty.BuildAccessors();

			if (Target is INotifyPropertyChanged)
			{
				converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();
				var sourceAccessors = sourceProperty.BuildAccessors();
				return Add(new TwoWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs, PropertyChangedEventArgs>(Source, sourceAccessors.Item1, sourceAccessors.Item2, nameof(INotifyPropertyChanged.PropertyChanged), Target, targetAccessors.Item1, targetAccessors.Item2, nameof(INotifyPropertyChanged.PropertyChanged), converter, (PropertyChangedEventArgs arg) => (arg.PropertyName == sourceAccessors.Item3), (PropertyChangedEventArgs arg) => (arg.PropertyName == targetAccessors.Item3)));

			}

			return this.Property(sourceProperty, targetAccessors.Item1, targetAccessors.Item2, converter);
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, string targetChangedEvent, IConverter<TSourceProperty, TTargetProperty> converter = null)
			where TTargetChangedEventArgs : EventArgs
		{
			if (Source is INotifyPropertyChanged)
			{
				converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();
				var sourceAccessors = sourceProperty.BuildAccessors();
				return Add(new TwoWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs, TTargetChangedEventArgs>(Source, sourceAccessors.Item1, sourceAccessors.Item2, nameof(INotifyPropertyChanged.PropertyChanged), Target, targetGetter, targetSetter, targetChangedEvent, converter, (PropertyChangedEventArgs arg) => (arg.PropertyName == sourceAccessors.Item3)));
			}

			return this.OneTime(sourceProperty, targetGetter, targetSetter, converter); // FIXME should work even if not an observable source
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, string targetChangedEvent, IConverter<TSourceProperty, TTargetProperty> converter = null)
			where TTargetChangedEventArgs : EventArgs
		{
			var targetAccessors = targetProperty.BuildAccessors();
			return this.Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(sourceProperty, targetAccessors.Item1, targetAccessors.Item2, targetChangedEvent, converter);
		}

		public override void Dispose()
		{
			if (!this.IsDisposed)
			{
				foreach (var binding in this.bindings)
				{
					binding.Dispose();
				}

				base.Dispose();
			}
		}
	}
}
