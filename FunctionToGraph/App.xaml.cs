using System;
using System.Threading;
using System.Windows;

namespace FunctionToGraph
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public ResourceDictionary LogicalResources => Resources.MergedDictionaries[0];
        
        
        private Mutex _appMutex;
        
        private void OnStartup(object sender, StartupEventArgs e)
        {
            _appMutex = new Mutex(true, AppDomain.CurrentDomain.FriendlyName, out bool createdNew);
            
            if (!createdNew)
            {
                Shutdown();
            }    
        }
    }
}