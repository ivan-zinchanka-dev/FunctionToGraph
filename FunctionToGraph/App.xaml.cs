using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using FunctionToGraph.Services;
using FunctionToGraph.Views;

namespace FunctionToGraph
{
    public partial class App : Application
    {
        public static App Instance => Current as App;
        
        public AppResourceModel ResourceModel { get; private set; }

        private StorageService _storageService;
        private MainWindow _mainWindow;
        
        public class AppResourceModel
        {
            private readonly ResourceDictionary _resourceDictionary;
            private const string GraphColorKey = "GraphColor";
            
            public AppResourceModel(ResourceDictionary resourceDictionary)
            {
                _resourceDictionary = resourceDictionary;
            }

            public Color GraphColor
            {
                get => (Color)_resourceDictionary[GraphColorKey];
                set
                {
                    _resourceDictionary[GraphColorKey] = value; 
                    OnGraphColorChanged?.Invoke(value);
                }
            }
            
            public event Action<Color> OnGraphColorChanged; 
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            PreventMultipleStartup();
            
            ResourceModel = new AppResourceModel(Resources.MergedDictionaries[0]);
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