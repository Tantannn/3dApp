using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Media3D;
using System.Windows.Threading;
using HelixToolkit.Wpf;
using Microsoft.Win32;
using System.IO;

namespace _3dApp;

/// <summary>
/// Interaction logic for MainWindow.xaml
/// </summary>
public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
        // Load3DModel(modelPath);
    }

    private void Load3DModel(string filePath)
    {
        ArgumentNullException.ThrowIfNull(filePath);
        var importer = new ModelImporter();

        ProgressBar.Visibility = Visibility.Visible;
        try
        {
            var models = importer.Load(filePath);
            HelixViewport3D.Children.Clear();

            var modelVisual = new ModelVisual3D { Content = models };
            HelixViewport3D.Children.Add(modelVisual);

            var light = new DirectionalLight(Colors.White, new Vector3D(-1, -1, -1));
            models.Children.Add(light);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Error loading model: {ex.Message}", "Error", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }
        finally
        {
            ProgressBar.Visibility = Visibility.Collapsed;
        }
    }

    private void LoadModelButton_Click(object sender, RoutedEventArgs routedEventArgs)
    {
        var openFileDialog = new OpenFileDialog
        {
            Title = "Select a 3D Model",
            Filter = "3D Model Files (*.obj;*.stl;*.3ds;*.ply)|*.obj;*.stl;*.3ds;*.ply|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() != true) return;
        var filePath = openFileDialog.FileName;
        Load3DModel(filePath);
    }
}