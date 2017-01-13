namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Windows.Input;

	/// <summary>
	/// A set of extensions that help with the use of Bindings.
	/// </summary>
	public static class Bindings
	{
		#region Binding collection

		private static List<IBinding> bindings = new List<IBinding>();

		/// <summary>
		/// A collection that keeps all created bindings in memory.
		/// </summary>
		public static IEnumerable<IBinding> All => bindings.ToArray();

		/// <summary>
		/// Removes all bindings that are not alive anymore from the global binding list.
		/// </summary>
		public static void Purge()
		{
			for (int i = 0; i < bindings.Count;)
			{
				var b = bindings[i];
				if (!b.IsAlive)
				{
					b.Dispose();
					bindings.RemoveAt(i);
				}
				else i++;
			}
		}

		/// <summary>
		/// Destroys all bindings.
		/// </summary>
		public static void Reset()
		{
			bindings = new List<IBinding>();
		}

		#endregion

		#region OneWay

		public static IBinding BindOneWay<TSource, TTarget, TSourceProperty, TTargetProperty, TSourceChangedEventArgs>(this TSource source, string sourceProperty, string sourceUpdateEvent, TTarget target, string targetProperty, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null)
			where TSourceChangedEventArgs : EventArgs
		{
			var b = new OneWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, TSourceChangedEventArgs>(source, sourceProperty, sourceUpdateEvent, target, targetProperty, converter, sourceEventFilter);
			bindings.Add(b);
			return b;
		}

		public static IBinding BindOneWay<TTarget, TSourceProperty, TTargetProperty>(this INotifyPropertyChanged source, string sourceProperty, TTarget target, string targetProperty, IConverter<TSourceProperty, TTargetProperty> converter)
		{
			return source.BindOneWay<INotifyPropertyChanged, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs>(sourceProperty, nameof(INotifyPropertyChanged.PropertyChanged), target, targetProperty, converter, (arg) => (arg.PropertyName == sourceProperty));
		}

		#endregion

		#region TwoWay

		public static IBinding BindTwoWay<TSource, TTarget, TSourceProperty, TTargetProperty, TSourceChangedEventArgs, TTargetChangedEventArgs>(this TSource source, string sourceProperty, string sourceUpdateEvent, TTarget target, string targetProperty, string targetUpdateEvent, IConverter<TSourceProperty, TTargetProperty> converter, Func<TSourceChangedEventArgs, bool> sourceEventFilter = null, Func<TTargetChangedEventArgs, bool> targetEventFilter = null)
			where TSourceChangedEventArgs : EventArgs
			where TTargetChangedEventArgs : EventArgs
		{
			var b = new TwoWayBinding<TSource, TTarget, TSourceProperty, TTargetProperty, TSourceChangedEventArgs, TTargetChangedEventArgs>(source, sourceProperty, sourceUpdateEvent, target, targetProperty, targetUpdateEvent, converter, sourceEventFilter, targetEventFilter);
			bindings.Add(b);
			return b;
		}

		public static IBinding BindTwoWay<TTarget, TSourceProperty, TTargetProperty, TTargetChangedEventArgs>(this INotifyPropertyChanged source, string sourceProperty, TTarget target, string targetProperty, string targetEvent, IConverter<TSourceProperty, TTargetProperty> converter)
			where TTargetChangedEventArgs : EventArgs
		{
			return source.BindTwoWay<INotifyPropertyChanged, TTarget, TSourceProperty, TTargetProperty, PropertyChangedEventArgs, TTargetChangedEventArgs>(sourceProperty, nameof(INotifyPropertyChanged.PropertyChanged), target, targetProperty, targetEvent, converter, (arg) => (arg.PropertyName == sourceProperty));
		}

		public static IBinding BindTwoWay<TSourceProperty, TTargetProperty>(this INotifyPropertyChanged source, string sourceProperty, INotifyPropertyChanged target, string targetProperty, IConverter<TSourceProperty, TTargetProperty> converter)
		{
			return source.BindTwoWay<INotifyPropertyChanged, INotifyPropertyChanged, TSourceProperty, TTargetProperty, PropertyChangedEventArgs, PropertyChangedEventArgs>(sourceProperty, nameof(INotifyPropertyChanged.PropertyChanged), target, targetProperty, nameof(INotifyPropertyChanged.PropertyChanged), converter, (arg) => (arg.PropertyName == sourceProperty),(arg) => (arg.PropertyName == targetProperty));
		}

		#endregion

		#region Commands

		public static IBinding Bind<TTarget, TTargetEventArgs>(this ICommand command, TTarget target, string targetEvent, Action<TTarget, bool> onExecuteChanged)
			where TTargetEventArgs : EventArgs
		{
			var b = new CommandBinding<TTarget, TTargetEventArgs>(command, target, targetEvent, onExecuteChanged);
			bindings.Add(b);
			return b;
		}

		#endregion
	}
}
