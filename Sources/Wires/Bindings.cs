namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Diagnostics;
	using System.Linq;
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
		/// Removes all bindings that are not alive anymore from the global binding list if the last purge 
		/// is older than the given interval.
		/// </summary>
		/// <param name="checkInterval">Purge consecutive minimum interval (if null, then forces a purge).</param>
		public static int Purge(TimeSpan? checkInterval = null)
		{
			int purged = 0;

			if ((checkInterval == null) || (lastPurge + checkInterval < DateTime.Now))
			{
				for (int i = 0; i < bindings.Count;)
				{
					var b = bindings[i];
					if (!b.IsAlive)
					{
						b.Dispose();
						bindings.RemoveAt(i);
						purged++;
					}
					else i++;
				}

				lastPurge = DateTime.Now;

				Debug.WriteLine($"[Bindings](Purge) {purged} removed, {bindings.Count} remaining");
			}

			return purged;
		}

		/// <summary>
		/// Destroys all bindings.
		/// </summary>
		public static void Reset()
		{
			foreach (var b in All)
			{
				b.Dispose();
			}

			bindings = new List<IBinding>();
		}

		private static DateTime lastPurge;

		public static void Unbind<TSource>(this TSource source, params object[] targets)
		{
			var toDispose = bindings.Where(c =>
			{
				object s, t;
				return c.TryGetSourceAndTarget(out s, out t) && (s == (object)source) && targets.Contains(t);
			}).ToArray();

			foreach (var item in toDispose)
			{
				item.Dispose();
			}
		}

		#endregion

		#region Binder

		public static TimeSpan PurgeInterval = TimeSpan.FromSeconds(15);

		public static Binder<TSource, TTarget> Bind<TSource, TTarget>(this TSource source, TTarget target) where TSource : class where TTarget : class
		{
			Purge(PurgeInterval);
			var binder = new Binder<TSource, TTarget>(source, target);
			bindings.Add(binder);
			return binder;
		}

		#endregion

	}
}
