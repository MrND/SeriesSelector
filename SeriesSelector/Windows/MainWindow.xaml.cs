using System.ComponentModel.Composition;
using System.Windows;
using System.Windows.Input;
using SeriesSelector.Frame;

namespace SeriesSelector.Windows
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    [Export(typeof(Window))]
    public partial class MainWindow : System.Windows.Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Open = new AdHocCommand(ExecuteOpen);
            DataContext = this;
        }

        public ICommand Open { get; private set; }

        private void ExecuteOpen(object parameter)
        {
            var viewModelKey = (string)parameter;
            var vm = BootStrapper.Resolve<object>(viewModelKey);
            mainForm.Content = vm;
        }

        private void WindowLoaded(object sender, RoutedEventArgs e)
        {
            this.WindowState = WindowState.Maximized;
        }
    }
}
