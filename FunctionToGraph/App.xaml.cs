using System;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using FunctionToGraph.Views;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace FunctionToGraph
{
    public partial class App : Application
    {
        public static App Instance => Current as App;
        
        public AppResourceModel ResourceModel { get; private set; }

        private readonly IHost _host;
        
        
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
        
        
        public App()
        {
           

            /*_host = Host.CreateDefaultBuilder()
                .ConfigureServices(services =>
                {

                })
                .Build();*/
        }
        
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
           
            ResourceModel = new AppResourceModel(Resources.MergedDictionaries[0]);
            
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