using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using HelpDesk.Models;

namespace HelpDesk.Views;

public partial class MainWindow : Window
{
    private List<Ticket> tickets;

    public MainWindow()
    {
        InitializeComponent();

        tickets = new List<Ticket>
        {
            new Ticket { Id = 1, Description = "Не работает подключение к интернету", Category = "Сеть", Priority = "Высокий", Status = "Новая", CreatedAt = DateTime.Now.AddHours(-4) },
            new Ticket { Id = 2, Description = "Ошибка при запуске Word", Category = "Программное обеспечение", Priority = "Средний", Status = "В работе", CreatedAt = DateTime.Now.AddDays(-1) },
            new Ticket { Id = 3, Description = "Не определяется принтер", Category = "Периферия", Priority = "Высокий", Status = "Выполнена", CreatedAt = DateTime.Now.AddDays(-2) },
            new Ticket { Id = 4, Description = "Шумит системный блок", Category = "Оборудование", Priority = "Низкий", Status = "Отклонена", CreatedAt = DateTime.Now.AddDays(-3) },
            new Ticket { Id = 5, Description = "Сбросить пароль пользователя", Category = "Учётная запись", Priority = "Средний", Status = "Новая", CreatedAt = DateTime.Now.AddHours(-2) }
        };

        CurrentUserText.Text = "Пользователь: admin";
        ShowTickets(tickets);
    }

    private Ticket? SelectedTicket => TicketsDataGrid.SelectedItem as Ticket;

    private void ShowTickets(List<Ticket> list)
    {
        TicketsDataGrid.ItemsSource = list;
        TicketCountText.Text = $"Количество заявок: {list.Count}";
    }

    private void ApplyFilters()
    {
        string searchText = SearchTextBox.Text.Trim();
        string selectedStatus = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Все";

        List<Ticket> result = tickets
            .Where(ticket =>
                (selectedStatus == "Все" || ticket.Status == selectedStatus) &&
                (string.IsNullOrWhiteSpace(searchText) ||
                 ticket.Description.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                 ticket.Category.Contains(searchText, StringComparison.CurrentCultureIgnoreCase) ||
                 ticket.Status.Contains(searchText, StringComparison.CurrentCultureIgnoreCase)))
            .ToList();

        ShowTickets(result);
    }

    private void SearchButton_Click(object sender, RoutedEventArgs e) => ApplyFilters();

    private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
    {
        if (IsLoaded)
            ApplyFilters();
    }

    private void StatusComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (IsLoaded)
            ApplyFilters();
    }

    private void ResetFilterButton_Click(object sender, RoutedEventArgs e)
    {
        SearchTextBox.Clear();
        StatusComboBox.SelectedIndex = 0;
        ApplyFilters();
    }

    private void AddTicketButton_Click(object sender, RoutedEventArgs e)
    {
        TicketCreateWindow window = new TicketCreateWindow { Owner = this };
        if (window.ShowDialog() == true && window.CreatedTicket != null)
        {
            window.CreatedTicket.Id = tickets.Count == 0 ? 1 : tickets.Max(ticket => ticket.Id) + 1;
            tickets.Add(window.CreatedTicket);
            ApplyFilters();
        }
    }

    private void EditTicketButton_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTicket == null)
        {
            MessageBox.Show("Выберите заявку для редактирования.", "HelpDesk", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        TicketEditWindow window = new TicketEditWindow(SelectedTicket) { Owner = this };
        if (window.ShowDialog() == true)
            ApplyFilters();
    }

    private void TicketsDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        if (SelectedTicket != null)
            EditTicketButton_Click(sender, e);
    }

    private void DeleteTicketButton_Click(object sender, RoutedEventArgs e)
    {
        if (SelectedTicket == null)
        {
            MessageBox.Show("Выберите заявку для удаления.", "HelpDesk", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        MessageBoxResult answer = MessageBox.Show(
            "Вы действительно хотите удалить выбранную заявку?",
            "Подтверждение удаления",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (answer == MessageBoxResult.Yes)
        {
            tickets.Remove(SelectedTicket);
            ApplyFilters();
        }
    }

    private void LogoutButton_Click(object sender, RoutedEventArgs e)
    {
        MessageBoxResult answer = MessageBox.Show(
            "Вы действительно хотите выйти из системы?",
            "Выход",
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (answer == MessageBoxResult.Yes)
        {
            LoginWindow loginWindow = new LoginWindow();
            Application.Current.MainWindow = loginWindow;
            loginWindow.Show();
            Close();
        }
    }
}
