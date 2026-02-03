using System.Windows;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class MainWindow : Window
{
    public MainWindow(MainViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
