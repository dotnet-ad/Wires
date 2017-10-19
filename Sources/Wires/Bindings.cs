namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq.Expressions;
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

		#region Binder

		public static Binder<TSource, TTarget> Bind<TSource, TTarget>(this TTarget target, TSource source) where TSource : class where TTarget : class
		{
			var binder = new Binder<TSource, TTarget>(source, target);
			bindings.Add(binder);
			return binder;
		}

		#endregion

		#region Commands

		public static IBinding Command<TTarget, TTargetEventArgs>(this Binder<ICommand,TTarget> binder, string targetEvent, Action<TTarget, bool> onExecuteChanged)
			where TTargetEventArgs : EventArgs
			where TTarget : class
		{
			var b = new CommandBinding<TTarget, TTargetEventArgs>(binder.Source, binder.Target, targetEvent, onExecuteChanged);
			bindings.Add(b);
			return b;
		}

		#endregion

	}
}
