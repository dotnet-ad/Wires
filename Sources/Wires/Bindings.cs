namespace Wires
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;

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
		/// Destroys all declared bindings.
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

	

		#endregion

		#region Actions


		public static TimeSpan PurgeInterval = TimeSpan.FromSeconds(15);

		/// <summary>
		/// Unbind all the bindings previously declared.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="targets">Targets.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		public static void Unbind<TSource>(this TSource source, params object[] targets)
		{
			var toDispose = bindings.Where(c =>
			{
				object s, t;
				return c.TryGetSourceAndTarget(out s, out t) && targets.Contains(t);
			}).ToArray();

			foreach (var item in toDispose)
			{
				item.Dispose();
			}
		}

		/// <summary>
		/// Start to bind properties from target to source.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="target">Target.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TTarget">The 2nd type parameter.</typeparam>
		public static Binder<TSource, TTarget> Bind<TSource, TTarget>(this TSource source, TTarget target) 
			where TSource : class 
			where TTarget : class
		{
			Purge(PurgeInterval);
			var binder = new Binder<TSource, TTarget>(source, target);
			bindings.Add(binder);
			return binder;
		}

		/// <summary>
		/// Unbind the target from the source, and restart a new set of bindings.
		/// </summary>
		/// <param name="source">Source.</param>
		/// <param name="target">Target.</param>
		/// <typeparam name="TSource">The 1st type parameter.</typeparam>
		/// <typeparam name="TTarget">The 2nd type parameter.</typeparam>
		public static Binder<TSource, TTarget> Rebind<TSource, TTarget>(this TSource source, TTarget target)
			where TSource : class
			where TTarget : class
		{
			source.Unbind(target);
			return source.Bind(target);
		}

		#endregion

	}
}
