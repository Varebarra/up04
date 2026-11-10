using System.Windows;
using System.Windows.Controls;
using HelpDesk.Models;
namespace HelpDesk.Views;
public partial class TicketEditWindow : Window
{
    private readonly Ticket _ticket;
    public TicketEditWindow(Ticket ticket)
    {
        InitializeComponent();
        _ticket = ticket;
        TicketNumberText.Text = $"Редактирование заявки №{ticket.Id}";
        SelectItem(CategoryComboBox, ticket.Category);
        DescriptionTextBox.Text = ticket.Description;
        SelectItem(PriorityComboBox, ticket.Priority);
        SelectItem(StatusComboBox, ticket.Status);
    }
    private static void SelectItem(ComboBox combo, string value)
    {
        foreach (ComboBoxItem item in combo.Items)
            if (item.Content?.ToString() == value) { combo.SelectedItem = item; return; }
    }
    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        string category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "";
        string description = DescriptionTextBox.Text.Trim();
        if (string.IsNullOrWhiteSpace(category) || string.IsNullOrWhiteSpace(description))
        {
            MessageBox.Show("Категория и описание должны быть заполнены.", "Проверка данных", MessageBoxButton.OK, MessageBoxImage.Warning);
            return;
        }
        _ticket.Category = category;
        _ticket.Description = description;
        _ticket.Priority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Средний";
        _ticket.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString() ?? "Новая";
        DialogResult = true;
    }
    private void CancelButton_Click(object sender, RoutedEventArgs e) => DialogResult = false;
}
