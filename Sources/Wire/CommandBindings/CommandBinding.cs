namespace Wire
{
	using System;
	using System.Windows.Input;


	public class CommandBinding<TTarget,TTargetEventArgs> : IBinding where TTargetEventArgs : EventArgs
	{
		public CommandBinding(ICommand command, TTarget target, string targetEvent, Action<TTarget, bool> onExecuteChanged)
		{
			this.SourceReference = new WeakReference(command);
			this.TargetReference = new WeakReference(target);
			UpdateCanExecute();
			this.onExecuteEvent = onExecuteChanged;
			this.targetEvent = target.AddWeakHandler<TTargetEventArgs>(targetEvent, this.OnClick);
			this.commandEvent = command.AddWeakHandler<EventArgs>(nameof(command.CanExecuteChanged), this.OnCanExecuteChanged);
		}

		readonly Action<TTarget, bool> onExecuteEvent;

		readonly WeakEventHandler<EventArgs> commandEvent;

		readonly WeakEventHandler<TTargetEventArgs> targetEvent;

		public string TargetProperty { get; private set; }

		public string SourceProperty { get; private set; }

		public WeakReference TargetReference { get; private set; }

		public WeakReference SourceReference { get; private set; }

		public TTarget Target
		{
			get
			{
				if (this.TargetReference.IsAlive)
					return (TTarget)this.TargetReference.Target;

				throw new InvalidOperationException($"{nameof(Target)} has been garbage collected and can't be used anymore");
			}
		}

		public ICommand Command
		{
			get
			{
				if (this.SourceReference.IsAlive)
					return (ICommand)this.SourceReference.Target;

				throw new InvalidOperationException($"{nameof(Command)} has been garbage collected and can't be used anymore");
			}
		}

		public bool IsAlive => TargetReference.IsAlive && SourceReference.IsAlive;

		private void OnClick(object sender, TTargetEventArgs e)
		{
			if (this.SourceReference.IsAlive)
			{
				var command = this.SourceReference.Target as ICommand;
				command.Execute(null);
			}
		}

		private void OnCanExecuteChanged(object sender, EventArgs e) => UpdateCanExecute();

		private void UpdateCanExecute()
		{
			if (this.IsAlive)
			{
				this.onExecuteEvent(this.Target, this.Command.CanExecute(null));
			}
		}

		public void Dispose()
		{
			this.targetEvent.Unsubscribe();
			this.commandEvent.Unsubscribe();
		}
	}
}
