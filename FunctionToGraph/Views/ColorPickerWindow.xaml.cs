using System;
using System.Windows;
using System.Windows.Media;

namespace FunctionToGraph.Views;

public partial class ColorPickerWindow : Window
{
    public event Action<Color> OnColorPicked;
    
    public ColorPickerWindow()
    {
        InitializeComponent();
    }

    private void OnColorPickedInternal(object sender, RoutedEventArgs e)
    {
        Color pickedColor = _colorPicker.SelectedColor;
        OnColorPicked?.Invoke(pickedColor);
    }
}