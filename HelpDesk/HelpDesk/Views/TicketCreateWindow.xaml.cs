using System.Windows;
using System.Windows.Controls;
using HelpDesk.Models;

namespace HelpDesk.Views;

public partial class TicketCreateWindow : Window
{
    public Ticket? CreatedTicket { get; private set; }

    public TicketCreateWindow()
    {
        InitializeComponent();
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
        string description = DescriptionTextBox.Text.Trim();
        string priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Средний";

        if (category == "Выберите категорию" || string.IsNullOrWhiteSpace(category))
        {
            MessageBox.Show("Выберите категорию.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            MessageBox.Show("Введите описание заявки.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }

        CreatedTicket = new Ticket
        {
            Category = category,
            Description = description,
            Priority = priority,
            Status = "Новая",
            CreatedAt = DateTime.Now
        };

        DialogResult = true;
    }

    private void CancelButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}
