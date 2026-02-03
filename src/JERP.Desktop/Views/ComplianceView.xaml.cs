using System.Windows.Controls;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class ComplianceView : UserControl
{
    public ComplianceView(ComplianceViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
