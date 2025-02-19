using System.Windows;

namespace _3dApp;

public partial class PickColorWindow : Window
{
    public string? SelectedColor { get; set; }
    public PickColorWindow()
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