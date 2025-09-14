using System;
using System.IO;
using System.Threading;
using System.Windows;
using FunctionDomain.Storage.Concrete;
using FunctionDomain.Storage.Contracts;
using FunctionToGraph.Views;

namespace FunctionToGraph
{
    public partial class App : Application
    {
        private IStorageService _storageService;
        private MainWindow _mainWindow;
        
        private const string AppFolderName = "FunctionToGraph";
        private static string AppDataPath => Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
        private static string StorageDirectoryPath => Path.Combine(AppDataPath, AppFolderName);
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PreventMultipleStartup();
            
            _storageService = new StorageService(StorageDirectoryPath);
            
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