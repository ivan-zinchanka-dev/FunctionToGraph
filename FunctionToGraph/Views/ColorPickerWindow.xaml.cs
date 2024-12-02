using System;
using System.Windows;
using System.Windows.Media;

namespace FunctionToGraph.Views;

public partial class ColorPickerWindow : Window
{
    private readonly Color _initialColor;
    private bool _colorSelected;
    
    public event Action<Color> OnColorPicked;
    
    public ColorPickerWindow(Color initialColor)
    {
        _initialColor = initialColor;
        InitializeComponent();

        Closed += OnClose;
    }

    private void OnPreview(object sender, RoutedEventArgs e)
    {
        OnColorPicked?.Invoke(_colorPicker.SelectedColor);
    }

    private void OnSelect(object sender, RoutedEventArgs e)
    {
        OnColorPicked?.Invoke(_colorPicker.SelectedColor);
        _colorSelected = true;
        Close();
    }
    
    private void OnClose(object sender, EventArgs e)
    {
        if (!_colorSelected)
        {
            OnColorPicked?.Invoke(_initialColor);
        }
    }
}