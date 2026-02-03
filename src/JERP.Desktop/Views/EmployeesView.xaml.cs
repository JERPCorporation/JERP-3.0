using System.Windows.Controls;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class EmployeesView : UserControl
{
    public EmployeesView(EmployeesViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
