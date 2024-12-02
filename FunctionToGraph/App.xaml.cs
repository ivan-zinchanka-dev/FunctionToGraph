using System;
using System.Threading;
using System.Windows;
using FunctionToGraph.Services;
using FunctionToGraph.Views;

namespace FunctionToGraph
{
    public partial class App : Application
    {
        private StorageService _storageService;
        private MainWindow _mainWindow;
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PreventMultipleStartup();
            
            _storageService = new StorageService();
            
            _mainWindow = new MainWindow(_storageService);
            _mainWindow.Show();
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