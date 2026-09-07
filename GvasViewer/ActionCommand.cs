using System.Windows.Input;

namespace GvasViewer
{
	class ActionCommand(Action<Object?> _action) : ICommand
	{
#pragma warning disable CS0067
		public event EventHandler? CanExecuteChanged;
#pragma warning restore CS0067

		public bool CanExecute(Object? parameter) => true;
		public void Execute(Object? parameter) => _action(parameter);
	}
}
