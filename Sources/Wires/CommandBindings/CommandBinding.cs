namespace Wires
{
	using System;
	using System.Windows.Input;


	public class CommandBinding<TSource, TTarget, TSourceEventArgs, TTargetEventArgs> : EventBinding<TSource,TTarget, TSourceEventArgs>, IBinding 
		where TTarget : class
		where TSource : class
		where TSourceEventArgs : EventArgs
		where TTargetEventArgs : EventArgs
	{
		public CommandBinding(TSource command, Func<TSource, ICommand> sourceGetter, string sourceEvent, Func<TSourceEventArgs,bool> sourceEventFilet, TTarget target, string targetEvent, Action<TTarget, bool> onExecuteChanged): base(command,target, sourceEvent, sourceEventFilet)
		{
			this.sourceGetter = sourceGetter;
			this.onExecuteEvent = onExecuteChanged;
			if (sourceEvent != null)
			{
				this.targetEvent = target.AddWeakHandler<TTargetEventArgs>(targetEvent, this.OnClick);
			}
			OnEvent();
		}

		readonly Func<TSource, ICommand> sourceGetter;

		readonly Action<TTarget, bool> onExecuteEvent;

		readonly WeakEventHandler<TTargetEventArgs> targetEvent;

		private WeakEventHandler<EventArgs> commandEvent;

		public string TargetProperty { get; private set; }

		public string SourceProperty { get; private set; }

		private bool TryGetCommand(out ICommand command)
		{
			TSource source;
			if (this.TryGetSource(out source))
			{
				command = sourceGetter(source);
				return true;
			}

			command = null;
			return false;
		}

		private void OnClick(object sender, TTargetEventArgs e)
		{
			ICommand command;
			if (this.TryGetCommand(out command))
			{
				command.Execute(null);
			}
		}

		private void OnCanExecuteChanged(object sender, EventArgs e) => UpdateCanExecute();

		private void UpdateCanExecute()
		{
			ICommand command;
			TTarget target;

			if (this.TryGetCommand(out command) && this.TargetReference.TryGetTarget(out target) && command !=null && target != null)
			{
				this.onExecuteEvent(target, command.CanExecute(null));
			}
		}

		public override void Dispose()
		{
			this.targetEvent?.Unsubscribe();
			this.commandEvent?.Unsubscribe();
			base.Dispose();
		}

		protected override void OnEvent()
		{
			if (this.commandEvent != null)
			{
				this.commandEvent.Unsubscribe();
				this.commandEvent = null;
			}

			ICommand command;
			if (this.TryGetCommand(out command) && command != null)
			{
				this.commandEvent = command.AddWeakHandler<EventArgs>(nameof(command.CanExecuteChanged), this.OnCanExecuteChanged);
				UpdateCanExecute();
			}
		}
	}
}
