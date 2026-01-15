using System.Windows;

namespace GvasViewer
{
	/// <summary>
	/// Interaction logic for MainWindow.xaml
	/// </summary>
	public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

		private void Window_PreviewDragOver(object sender, DragEventArgs e)
		{
			e.Handled = e.Data.GetDataPresent(DataFormats.FileDrop);
		}

		private void Window_Drop(object sender, DragEventArgs e)
		{
			String[]? files = e.Data.GetData(DataFormats.FileDrop) as String[];
			if (files == null) return;

			var vm = DataContext as ViewModel.MainViewModel;
			if (vm == null) return;

			vm.LoadFile(files[0]);
		}
	}
}