using System;
using System.Threading;
using System.Windows;

namespace FunctionToGraph
{
    public partial class App : Application
    {
        public ResourceDictionary LogicalResources => Resources.MergedDictionaries[0];
        
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            PreventMultipleStartup();

            new MainWindow().Show();
        }

        private void PreventMultipleStartup()
        {
            Mutex appMutex = new Mutex(true, AppDomain.CurrentDomain.FriendlyName, out bool createdNew);
            
            if (!createdNew)
            {
                Shutdown();
            }
        }
    }
}