using System.ComponentModel.Composition;
using System.Diagnostics;
using SeriesSelector.Services;

namespace SeriesSelector
{
    [Export(typeof(IStartup))]
    public class SyncStartup : IStartup
    {
        public void Start()
        {
            Debug.WriteLine("Greetings from sync");
        }
    }
}