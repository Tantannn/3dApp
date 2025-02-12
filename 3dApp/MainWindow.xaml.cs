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
            Filter = "3D Model Files (*.obj;*.stl;*.3ds;*.ply;*.gltf)|*.obj;*.stl;*.3ds;*.ply;*.gltf|All Files (*.*)|*.*"
        };

        if (openFileDialog.ShowDialog() != true) return;
        var filePath = openFileDialog.FileName;
        Load3DModel(filePath);
    }

    private void SaveModelButton_Click(object sender, RoutedEventArgs e)
    {
        var saveFileDialog = new SaveFileDialog
        {
            Title = "Save 3D Model",
            Filter = "Wavefront OBJ (*.obj)|*.obj|All Files (*.*)|*.*"
        };

        if (saveFileDialog.ShowDialog() != true) return;

        var filePath = saveFileDialog.FileName;
        Save3DModel(filePath);
    }

        private void Save3DModel(string filePath)
        {
            if (HelixViewport3D.Children.Count == 0)
            {
                MessageBox.Show("No model to save!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Find the ModelVisual3D in HelixViewport3D's children
            var modelVisual = HelixViewport3D.Children.OfType<ModelVisual3D>().FirstOrDefault();

            if (modelVisual?.Content == null)
            {
                MessageBox.Show("No model to save!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (modelVisual.Content is not { } model)
            {
                MessageBox.Show("No model to save!", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var exporter = new ObjExporter();

            try
            {
                using (var stream = File.Create(filePath))
                {
                    exporter.Export(model, stream);
                }

                MessageBox.Show("Model saved successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving model: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
        private void Viewport_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var mousePosition = e.GetPosition(HelixViewport3D);
            var result = VisualTreeHelper.HitTest(HelixViewport3D, mousePosition);
            
            if (result is RayMeshGeometry3DHitTestResult { ModelHit: GeometryModel3D geometryModel })
            {
                geometryModel.Material = new DiffuseMaterial(new SolidColorBrush(Colors.Red));
            }
        }
}
