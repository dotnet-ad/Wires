namespace Wires
{
	using System;
	using System.Windows.Input;


	public class CommandBinding<TTarget,TTargetEventArgs> : IBinding where TTargetEventArgs : EventArgs
		where TTarget : class
	{
		public CommandBinding(ICommand command, TTarget target, string targetEvent, Action<TTarget, bool> onExecuteChanged)
		{
			this.SourceReference = new WeakReference<ICommand>(command);
			this.TargetReference = new WeakReference<TTarget>(target);
			this.onExecuteEvent = onExecuteChanged;
			this.targetEvent = target.AddWeakHandler<TTargetEventArgs>(targetEvent, this.OnClick);
			this.commandEvent = command.AddWeakHandler<EventArgs>(nameof(command.CanExecuteChanged), this.OnCanExecuteChanged);
			UpdateCanExecute();
		}

		bool isDisposed;

		readonly Action<TTarget, bool> onExecuteEvent;

		readonly WeakEventHandler<EventArgs> commandEvent;

		readonly WeakEventHandler<TTargetEventArgs> targetEvent;

		public string TargetProperty { get; private set; }

		public string SourceProperty { get; private set; }

		public WeakReference<TTarget> TargetReference { get; private set; }

		public WeakReference<ICommand> SourceReference { get; private set; }

		public bool IsAlive
		{
			get
			{
				ICommand source;
				TTarget target;

				return this.SourceReference.TryGetTarget(out source) && this.TargetReference.TryGetTarget(out target) && !this.isDisposed;
			}
		}

		private void OnClick(object sender, TTargetEventArgs e)
		{
			ICommand source;

			if (this.SourceReference.TryGetTarget(out source))
			{
				source.Execute(null);
			}
		}

		private void OnCanExecuteChanged(object sender, EventArgs e) => UpdateCanExecute();

		private void UpdateCanExecute()
		{
			ICommand source;
			TTarget target;

			if (this.SourceReference.TryGetTarget(out source) && this.TargetReference.TryGetTarget(out target) && source !=null && target != null)
			{
				this.onExecuteEvent(target, source.CanExecute(null));
			}
		}

		public void Dispose()
		{
			this.targetEvent.Unsubscribe();
			this.commandEvent.Unsubscribe();
			this.isDisposed = true;
		}
	}
}
