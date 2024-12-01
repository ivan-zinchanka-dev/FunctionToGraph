using System;
using System.Windows;
using System.Windows.Media;

namespace FunctionToGraph.Views;

public partial class GraphColorWindow : Window
{
    public GraphColorWindow()
    {
        InitializeComponent();
        Console.WriteLine(nameof(GraphColorWindow));
    }

    private void OnColorChanged(object sender, RoutedEventArgs e)
    {
        Color selectedColor = _colorPicker.SelectedColor;
        App.Instance.ResourceModel.GraphColor = selectedColor;
    }
}