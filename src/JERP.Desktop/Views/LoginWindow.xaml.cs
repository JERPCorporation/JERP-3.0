using System.Windows;
using System.Windows.Controls;
using JERP.Desktop.ViewModels;

namespace JERP.Desktop.Views;

public partial class LoginWindow : Window
{
    public LoginWindow(LoginViewModel viewModel)
    {
        InitializeComponent();
        DataContext = viewModel;
        
        PasswordBox.PasswordChanged += (s, e) =>
        {
            if (DataContext is LoginViewModel vm)
            {
                vm.Password = PasswordBox.Password;
            }
        };
    }
}
