using System.Windows.Controls;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class DashboardView : UserControl
{
    public DashboardView(DashboardViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
