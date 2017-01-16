using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;

namespace Wires
{
	public class Binder<TSource, TTarget> : IBinding
		where TSource : class
		where TTarget : class
	{
		public Binder(TSource source, TTarget target)
		{
			this.Source = source;
			this.Target = target;
		}

		public TSource Source { get; private set; }

		public TTarget Target { get; private set; }

		public bool IsAlive => this.bindings.Any(b => b.IsAlive);

		private List<IBinding> bindings = new List<IBinding>();

		private IBinding Add(IBinding binding)
		{
			this.bindings.Add(binding);
			return this;
		}

		#region OneTime

		private IBinding OneTime<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();
			var targetAccessors = targetProperty.BuildAccessors();

			return Add(new OneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceAccessors.Item1, Target, targetAccessors.Item1, targetAccessors.Item2, converter));
		}

		private IBinding OneTime<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();

			return Add(new OneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceAccessors.Item1, Target, targetGetter, targetSetter, converter));
		}

		#endregion

		public IBinding Property<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			if (Source is INotifyPropertyChanged)
			{
				converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();
				var sourceAccessors = sourceProperty.BuildAccessors();
				return Add(new OneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs>(Source, sourceAccessors.Item1, sourceAccessors.Item2, nameof(INotifyPropertyChanged.PropertyChanged), Target, targetGetter, targetSetter, converter, (PropertyChangedEventArgs arg) => (arg.PropertyName == sourceAccessors.Item3)));
			}

			return this.OneTime(sourceProperty, targetGetter, targetSetter, converter);
		}

		public IBinding Property<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, TTargetProperty> converter = null)
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

		public IBinding Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, string targetChangedEvent, IConverter<TSourceProperty, TTargetProperty> converter = null)
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

		public IBinding Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, string targetChangedEvent, IConverter<TSourceProperty, TTargetProperty> converter = null)
			where TTargetChangedEventArgs : EventArgs
		{
			var targetAccessors = targetProperty.BuildAccessors();
			return this.Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(sourceProperty, targetAccessors.Item1, targetAccessors.Item2, targetChangedEvent, converter);
		}

		public void Dispose()
		{
			throw new NotImplementedException();
		}
	}
}
