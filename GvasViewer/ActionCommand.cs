using System.Windows.Input;

namespace GvasViewer
{
	class ActionCommand : ICommand
	{
#pragma warning disable CS0067
		public event EventHandler? CanExecuteChanged
		{
			add { CommandManager.RequerySuggested += value; }
			remove { CommandManager.RequerySuggested -= value; }
		}
#pragma warning restore CS0067

		private readonly Action<Object?> mAction;
		private readonly Predicate<Object?> mPredicate;
		public ActionCommand(Action<Object?> action, Predicate<Object?> predicate)
		{
			mAction = action;
			mPredicate = predicate;
		}

		public bool CanExecute(Object? parameter) => mPredicate.Invoke(parameter);
		public void Execute(Object? parameter) => mAction.Invoke(parameter);
	}
}
