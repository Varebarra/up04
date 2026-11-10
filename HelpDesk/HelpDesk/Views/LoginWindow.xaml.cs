using System.Windows;

namespace HelpDesk.Views;

public partial class LoginWindow : Window
{
    public LoginWindow()
    {
        InitializeComponent();
    }

    private void LoginButton_Click(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(LoginTextBox.Text))
        {
            MessageBox.Show("Введите логин.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(PasswordInput.Password))
        {
            MessageBox.Show("Введите пароль.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        string login = LoginTextBox.Text.Trim();
        string password = PasswordInput.Password;

        if (login == "admin" && password == "12345")
        {
            MainWindow mainWindow = new MainWindow();
            Application.Current.MainWindow = mainWindow;
            mainWindow.Show();
            Close();
        }
        else
        {
            MessageBox.Show("Неверный логин или пароль.", "Ошибка авторизации", MessageBoxButton.OK, MessageBoxImage.Error);
        }
    }
}
