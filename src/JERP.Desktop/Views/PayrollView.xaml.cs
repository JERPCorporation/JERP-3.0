using System.Windows.Controls;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class PayrollView : UserControl
{
    public PayrollView(PayrollViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
