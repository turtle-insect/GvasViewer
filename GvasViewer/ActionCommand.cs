using System.Windows.Input;

namespace GvasViewer
{
	internal class ActionCommand : ICommand
	{
#pragma warning disable CS0067
		public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

		private readonly Action<Object?> mAction;
		public ActionCommand(Action<Object?> action) => mAction = action;
		public bool CanExecute(object? parameter) => true;
		public void Execute(object? parameter) => mAction.Invoke(parameter);
	}
}
