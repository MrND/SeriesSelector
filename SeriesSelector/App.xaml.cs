using System.Windows;
using SeriesSelector.Frame;

namespace SeriesSelector
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            BootStrapper.Bootstrap();
            var wdw = BootStrapper.Resolve<Window>();
            wdw.Show();
        }
    }
}
