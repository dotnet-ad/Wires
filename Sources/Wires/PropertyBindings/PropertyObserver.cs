using System;
using System.ComponentModel;
using System.Linq;

namespace Wires
{
	public class PropertyObserver<TSource, TTarget> : EventBinding<TSource,TTarget, PropertyChangedEventArgs>
		where TSource : class
		where TTarget : class
	{
		public PropertyObserver(TSource source, TTarget target, Action<TSource, TTarget> onEvent, params string[] properties) : base(source, target, nameof(INotifyPropertyChanged), x => properties.Contains(x.PropertyName))
		{
			this.onEvent = onEvent;
		}

		private Action<TSource, TTarget> onEvent;

		protected override void OnEvent() => this.onEvent(this.Source, this.Target);
	}
}
