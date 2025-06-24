using System;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.WPFDataSetTableAdapters;

namespace UserManagementApp
{
    public partial class RolesWindow : Window
    {
        private readonly Role_uTableAdapter _rolesAdapter = new Role_uTableAdapter();
        private int? _selectedRoleId = null;

        public RolesWindow()
        {
            InitializeComponent();
            LoadRoles();
        }

        public void LoadRoles()
        {
            var roles = _rolesAdapter.GetData();
            RolesDataGrid.ItemsSource = roles;
        }

        private void AddRoleButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new RoleEditWindow(this);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void UpdateRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoleId == null)
            {
                MessageBox.Show("Выберите роль для редактирования", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new RoleEditWindow(this, _selectedRoleId);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void DeleteRoleButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedRoleId == null)
            {
                MessageBox.Show("Выберите роль для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить эту роль?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var roleRow = _rolesAdapter.GetData().FirstOrDefault(r => r.RoleID == _selectedRoleId);

                    if (roleRow == null)
                    {
                        MessageBox.Show("Роль не найдена", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _rolesAdapter.Delete(
                        _selectedRoleId.Value,
                        roleRow.Role_name
                    );

                    LoadRoles();
                    RoleNameTextBox.Clear();
                    _selectedRoleId = null;
                    MessageBox.Show("Роль успешно удалена", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}\nРоль может быть связана с пользователями.",
                        "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void RolesDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RolesDataGrid.SelectedItem is DataRowView selectedRow)
            {
                _selectedRoleId = (int)selectedRow["RoleID"];
                RoleNameTextBox.Text = selectedRow["Role_name"].ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            var mainMenu = Application.Current.Windows.OfType<MainMenu>().FirstOrDefault();

            if (mainMenu != null)
            {
                mainMenu.Show();
            }
            else
            {

                mainMenu = new MainMenu();
                mainMenu.Show();
            }

            this.Close();
        }
    }
}