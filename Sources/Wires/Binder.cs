namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;
	using System.Linq.Expressions;
	using System.Threading.Tasks;
	using System.Windows.Input;

	/// <summary>
	/// A binder allows the creation of one or more bindings from a source to a target.
	/// </summary>
	public class Binder<TSource, TTarget> : Binding<TSource,TTarget>, IBinding
		where TSource : class
		where TTarget : class
	{
		public Binder(TSource source, TTarget target) : base(source,target)
		{
		}

		public override bool IsAlive => !this.IsDisposed && this.bindings.Any(b => b.IsAlive);

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

		public Binder<TTarget, TSource> Invert()
		{
			var cast = new Binder<TTarget, TSource>(Target,Source);
			this.Add(cast);
			return cast;
		}

		#region OneTime

		private Binder<TSource, TTarget> OneTime<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();
			var targetAccessors = targetProperty.BuildAccessors();

			return this.OneTime(sourceAccessors.Item1, targetAccessors.Item1, targetAccessors.Item2, converter);
		}

		private Binder<TSource, TTarget> OneTime<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();

			return this.OneTime(sourceAccessors.Item1, targetGetter, targetSetter, converter);
		}

		private Binder<TSource, TTarget> OneTime<TSourceProperty, TTargetProperty>(Func<TSource, TSourceProperty> sourceGetter , Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			return Add(new OneTimeBinding<TSource, TTarget, TSourceProperty, TTargetProperty>(Source, sourceGetter, Target, targetGetter, targetSetter, converter));
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

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty>(Func<TSource, TSourceProperty> sourceGetter, Action<TSource, TSourceProperty> sourceSetter , Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, string propertyName, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			if (Source is INotifyPropertyChanged)
			{
				converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();
				return Add(new OneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs>(Source, sourceGetter, sourceSetter, nameof(INotifyPropertyChanged.PropertyChanged), Target, targetGetter, targetSetter, converter, (PropertyChangedEventArgs arg) => (arg.PropertyName == propertyName)));
			}

			return this.OneTime(sourceGetter, targetGetter, targetSetter, converter);
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			var sourceAccessors = sourceProperty.BuildAccessors();
			return this.Property(sourceAccessors.Item1, sourceAccessors.Item2, targetGetter, targetSetter, sourceAccessors.Item3, converter);
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			var targetAccessors = targetProperty.BuildAccessors();
			return this.Property(sourceProperty , targetAccessors.Item1, targetAccessors.Item2, converter);
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Func<TTarget, TTargetProperty> targetGetter, Action<TTarget, TTargetProperty> targetSetter, string targetChangedEvent, IConverter<TSourceProperty, TTargetProperty> converter = null, string targetPropertyName = null)
			where TTargetChangedEventArgs : EventArgs
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var r = this.Property(sourceProperty, targetGetter, targetSetter, converter);

			var sourceAccessors = sourceProperty.BuildAccessors();
			this.Invert().Add(new OneWayBinding<TTarget, TSource, TTargetProperty, TSourceProperty, TTargetChangedEventArgs>(Target, targetGetter, targetSetter, targetChangedEvent, Source, sourceAccessors.Item1, sourceAccessors.Item2, converter.Inverse()));
			return r;
		}

		public Binder<TSource, TTarget> Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Expression<Func<TTarget, TTargetProperty>> targetProperty, string targetChangedEvent, IConverter<TSourceProperty, TTargetProperty> converter = null)
			where TTargetChangedEventArgs : EventArgs
		{
			var targetAccessors = targetProperty.BuildAccessors();
			return this.Property<TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(sourceProperty, targetAccessors.Item1, targetAccessors.Item2, targetChangedEvent, converter, targetAccessors.Item3);
		}

		#region Commands

		public Binder<TSource, TTarget> Command<TTargetEventArgs>(Expression<Func<TSource, ICommand>> sourceProperty, string targetEvent, Action<TTarget, bool> onExecuteChanged)
			where TTargetEventArgs : EventArgs
		{
			string sourceEvent = null;

			if (this.Source is INotifyPropertyChanged)
			{
				sourceEvent = nameof(INotifyPropertyChanged.PropertyChanged);
			}

			var sourceAccessors = sourceProperty.BuildAccessors();
			return this.Add(new CommandBinding<TSource,TTarget, PropertyChangedEventArgs, TTargetEventArgs>(this.Source, sourceAccessors.Item1, sourceEvent, (PropertyChangedEventArgs arg) => (arg.PropertyName == sourceAccessors.Item3), this.Target, targetEvent, onExecuteChanged));
		}

		#endregion

		#region Observers

		public Binder<TSource, TTarget> ObserveProperty<TSourceProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Action<TSource, TTarget, TSourceProperty> action)
		{
			return this.ObserveProperty<TSourceProperty, TSourceProperty>(sourceProperty, action);
		}

		/// <summary>
		/// Observes the property change and executes the given callback each time it changes. The action is also executed at initialization.
		/// </summary>
		/// <returns>The property.</returns>
		/// <param name="sourceProperty">Source property.</param>
		/// <param name="action">Action.</param>
		/// <typeparam name="TSourceProperty">The 1st type parameter.</typeparam>
		public Binder<TSource, TTarget> ObserveProperty<TSourceProperty, TTargetProperty>(Expression<Func<TSource, TSourceProperty>> sourceProperty, Action<TSource, TTarget, TTargetProperty> action, IConverter<TSourceProperty, TTargetProperty> converter = null)
		{
			converter = converter ?? Converters.Default<TSourceProperty, TTargetProperty>();

			var sourceAccessors = sourceProperty.BuildAccessors();
			Action<TSource, TTarget> onEvent = (s, t) => action(s, t, converter.Convert(sourceAccessors.Item1(s)));

			// Initialization
			onEvent(this.Source, this.Target);

			// Changes
			if (this.Source is INotifyPropertyChanged)
			{
				this.Add(new RelayEventBinding<TSource, TTarget, PropertyChangedEventArgs>(this.Source, this.Target, nameof(INotifyPropertyChanged.PropertyChanged), onEvent, (a) => (a.PropertyName == sourceAccessors.Item3)));
			}

			return this;
		}

		#endregion

		public Binder<TSource, TNewTarget> Bind<TNewTarget>(TNewTarget target) 
			where TNewTarget : class
		{
			return this.Source.Bind(target);
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
