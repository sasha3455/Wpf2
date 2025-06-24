using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WpfApp2.WPFDataSetTableAdapters;

namespace UserManagementApp
{
    public partial class UsersWindow : Window
    {
        private readonly UsersTableAdapter _usersAdapter = new UsersTableAdapter();
        private readonly Role_uTableAdapter _rolesAdapter = new Role_uTableAdapter();
        private int? _selectedUserId = null;

        public UsersWindow()
        {
            InitializeComponent();
            LoadData();
        }

        public class UserView
        {
            public int User_ID { get; set; }
            public string Name_u { get; set; }
            public string Surname { get; set; }
            public string Lastname { get; set; }
            public string Email { get; set; }
            public string RoleName { get; set; }
            public string PhoneNumber { get; set; }
            public string Address { get; set; }
            public string BankCard { get; set; }
        }

        public void LoadData()
        {
            var users = _usersAdapter.GetData();
            var roles = _rolesAdapter.GetData();

            var userViews = new List<UserView>();

            foreach (var user in users)
            {
                var role = roles.FirstOrDefault(r => r.RoleID == user.RoleID);
                userViews.Add(new UserView
                {
                    User_ID = user.User_ID,
                    Name_u = user.Name_u,
                    Surname = user.Surname,
                    Lastname = user.Lastname,
                    Email = user.Email,
                    RoleName = role?.Role_name,
                    PhoneNumber = user.phone_number,
                    Address = user.Address_u,
                    BankCard = user.Bank_card
                });
            }

            UsersDataGrid.ItemsSource = userViews;
            RoleComboBox.ItemsSource = roles;
            RoleComboBox.DisplayMemberPath = "Role_name";
            RoleComboBox.SelectedValuePath = "RoleID";
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new UserEditWindow(this);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void UpdateButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUserId == null)
            {
                MessageBox.Show("Выберите пользователя для редактирования", "Ошибка",
                               MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var editWindow = new UserEditWindow(this, _selectedUserId);
            editWindow.Owner = this;
            editWindow.ShowDialog();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (_selectedUserId == null)
            {
                MessageBox.Show("Выберите пользователя для удаления", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Вы уверены, что хотите удалить этого пользователя?", "Подтверждение",
                MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    var userRow = _usersAdapter.GetData().FirstOrDefault(u => u.User_ID == _selectedUserId);

                    if (userRow == null)
                    {
                        MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    _usersAdapter.Delete(
                        _selectedUserId.Value,
                        userRow.Name_u,
                        userRow.Surname,
                        userRow.Lastname,
                        userRow.Email,
                        userRow.Password_u,
                        userRow.phone_number ?? "",  
                        userRow.Address_u ?? "",      
                        userRow.Bank_card ?? "",      
                        userRow.RoleID
                    );

                    LoadData();
                    ClearForm();
                    MessageBox.Show("Пользователь успешно удален", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                    _selectedUserId = null;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Ошибка при удалении: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void UsersDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UsersDataGrid.SelectedItem is UserView selectedUser)
            {
                _selectedUserId = selectedUser.User_ID;
                NameTextBox.Text = selectedUser.Name_u;
                SurnameTextBox.Text = selectedUser.Surname;
                LastnameTextBox.Text = selectedUser.Lastname;
                EmailTextBox.Text = selectedUser.Email;

                var userRow = _usersAdapter.GetData().FirstOrDefault(u => u.User_ID == _selectedUserId);
                if (userRow != null)
                {
                    PasswordBox.Password = userRow.Password_u;
                    PhoneTextBox.Text = userRow.phone_number ?? "";
                    AddressTextBox.Text = userRow.Address_u ?? "";
                    BankCardTextBox.Text = userRow.Bank_card ?? "";
                    RoleComboBox.SelectedValue = userRow.RoleID;
                }
            }
        }

        private void ValidateUserInput()
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
                throw new Exception("Имя не может быть пустым");

            if (string.IsNullOrWhiteSpace(SurnameTextBox.Text))
                throw new Exception("Фамилия не может быть пустой");

            if (string.IsNullOrWhiteSpace(EmailTextBox.Text))
                throw new Exception("Email не может быть пустым");
            else if (!EmailTextBox.Text.Contains("@"))
                throw new Exception("Email должен содержать символ @");

            if (PasswordBox.Password.Length < 6)
                throw new Exception("Пароль должен содержать не менее 6 символов");

            if (RoleComboBox.SelectedValue == null)
                throw new Exception("Выберите роль пользователя");
        }

        private void ClearForm()
        {
            NameTextBox.Clear();
            SurnameTextBox.Clear();
            LastnameTextBox.Clear();
            EmailTextBox.Clear();
            PasswordBox.Clear();
            PhoneTextBox.Clear();
            AddressTextBox.Clear();
            BankCardTextBox.Clear();
            RoleComboBox.SelectedIndex = -1;
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