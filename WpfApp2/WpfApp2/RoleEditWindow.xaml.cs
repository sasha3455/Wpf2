using System;
using System.Linq;
using System.Windows;
using WpfApp2.WPFDataSetTableAdapters;

namespace UserManagementApp
{
    public partial class RoleEditWindow : Window
    {
        private readonly Role_uTableAdapter _rolesAdapter = new Role_uTableAdapter();
        private int? _roleId;
        private RolesWindow _parentWindow;

        public string WindowTitle => _roleId == null ? "Добавление роли" : "Редактирование роли";

        public RoleEditWindow(RolesWindow parentWindow, int? roleId = null)
        {
            InitializeComponent();
            _parentWindow = parentWindow;
            _roleId = roleId;
            DataContext = this;
            LoadRoleData();
        }

        private void LoadRoleData()
        {
            if (_roleId != null)
            {
                var role = _rolesAdapter.GetData().FirstOrDefault(r => r.RoleID == _roleId);
                if (role != null)
                {
                    RoleNameTextBox.Text = role.Role_name;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(RoleNameTextBox.Text))
                {
                    throw new Exception("Введите название роли");
                }

                if (_roleId == null)
                {
                    _rolesAdapter.Insert(RoleNameTextBox.Text);
                }
                else
                {
                    var roleRow = _rolesAdapter.GetData().First(r => r.RoleID == _roleId);
                    _rolesAdapter.Update(
                        RoleNameTextBox.Text,
                        _roleId.Value,
                        roleRow.Role_name,
                        _roleId.Value
                    );
                }

                _parentWindow.LoadRoles();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}