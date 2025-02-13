using System.Windows;

namespace _3dApp;

public partial class Color : Window
{
    public string? SelectedColor { get; set; }
    public Color()
    {
        InitializeComponent();

    }
    
    private void ApplyColor_Click(object sender, RoutedEventArgs e)
    {
        SelectedColor = ColorInputBox.Text.Trim();
        DialogResult = true;
        Close();
    }
}