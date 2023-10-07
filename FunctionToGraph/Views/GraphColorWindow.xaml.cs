using System.Windows;
using System.Windows.Media;
using ColorPicker.Models;

namespace FunctionToGraph.Views;

public partial class GraphColorWindow : Window
{
    public GraphColorWindow()
    {
        InitializeComponent();
    }

    private void OnColorChanged(object sender, RoutedEventArgs e)
    {
        Color selectedColor = _colorPicker.SelectedColor;
        
        App app = (App)Application.Current;
        app.LogicalResources["GraphColor"] = selectedColor;
    }
}