using System.Windows.Controls;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class TimesheetsView : UserControl
{
    public TimesheetsView(TimesheetsViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
    }
}
