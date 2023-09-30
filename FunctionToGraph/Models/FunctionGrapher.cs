using System;
using System.Windows;
using System.Windows.Controls;
using ScottPlot;

namespace FunctionToGraph.Models;

public class FunctionGrapher : DependencyObject
{
    public static readonly DependencyProperty FunctionProperty;

    public Plot Plot { set; get; }

    public string Function
    {
        get => (string)GetValue(FunctionProperty);
        set => SetValue(FunctionProperty, value);
    }
    
    
    
    static FunctionGrapher()
    {
        FrameworkPropertyMetadata propertyMetadata = new FrameworkPropertyMetadata("Pow(x, 3)", 
            FrameworkPropertyMetadataOptions.AffectsMeasure |
            FrameworkPropertyMetadataOptions.AffectsRender, 
            new PropertyChangedCallback(OnValueChanged),
            new CoerceValueCallback(CorrectValueIfNeed));
        
        FunctionProperty = DependencyProperty.Register("Function", typeof(string), typeof(FunctionGrapher), 
            propertyMetadata, new ValidateValueCallback(ValidateValue));
        
    }
    
    
    
    private static bool ValidateValue(object value)
    {
        
        Console.WriteLine("Validate: " + value);
        
        return true;
        
        //it ValidateValue() -> CorrectValueIfNeed() -> OnValueChanged()
        //else break
    }
    
    private static object CorrectValueIfNeed(DependencyObject depObj, object baseValue)
    {
        // sin() -> Sin() 
        
        Console.WriteLine("Correct " + baseValue);
        
        return baseValue;
    }
    
    private static void OnValueChanged(DependencyObject depObj, DependencyPropertyChangedEventArgs e)
    { 
        Console.WriteLine("Change");
    }
    
}