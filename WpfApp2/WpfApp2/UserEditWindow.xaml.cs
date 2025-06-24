using System;
using System.Linq;
using System.Windows;
using WpfApp2.WPFDataSetTableAdapters;

namespace UserManagementApp
{
    public partial class UserEditWindow : Window
    {
        private readonly UsersTableAdapter _usersAdapter = new UsersTableAdapter();
        private readonly Role_uTableAdapter _rolesAdapter = new Role_uTableAdapter();
        private int? _userId;
        private UsersWindow _parentWindow;

        public string WindowTitle => _userId == null ? "Добавление пользователя" : "Редактирование пользователя";

        public UserEditWindow(UsersWindow parentWindow, int? userId = null)
        {
            InitializeComponent();
            _parentWindow = parentWindow;
            _userId = userId;
            DataContext = this;
            LoadRoles();
            LoadUserData();
        }

        private void LoadRoles()
        {
            var roles = _rolesAdapter.GetData();
            RoleComboBox.ItemsSource = roles;
        }

        private void LoadUserData()
        {
            if (_userId != null)
            {
                var user = _usersAdapter.GetData().FirstOrDefault(u => u.User_ID == _userId);
                if (user != null)
                {
                    NameTextBox.Text = user.Name_u;
                    SurnameTextBox.Text = user.Surname;
                    LastnameTextBox.Text = user.Lastname;
                    EmailTextBox.Text = user.Email;
                    PasswordBox.Password = user.Password_u;
                    ConfirmPasswordBox.Password = user.Password_u;
                    PhoneTextBox.Text = user.phone_number;
                    AddressTextBox.Text = user.Address_u;
                    BankCardTextBox.Text = user.Bank_card;
                    RoleComboBox.SelectedValue = user.RoleID;
                }
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ValidateInput();

                string phoneNumber = string.IsNullOrEmpty(PhoneTextBox.Text) ? null : PhoneTextBox.Text;
                string address = string.IsNullOrEmpty(AddressTextBox.Text) ? null : AddressTextBox.Text;
                string bankCard = string.IsNullOrEmpty(BankCardTextBox.Text) ? null : BankCardTextBox.Text;

                if (_userId == null)
                {

                    _usersAdapter.Insert(
                        NameTextBox.Text,
                        SurnameTextBox.Text,
                        LastnameTextBox.Text,
                        EmailTextBox.Text,
                        PasswordBox.Password,
                        phoneNumber,
                        address,
                        bankCard,
                        (int)RoleComboBox.SelectedValue
                    );
                }
                else
                {

                    var userRow = _usersAdapter.GetData().First(u => u.User_ID == _userId);
                    _usersAdapter.Update(
                        NameTextBox.Text,
                        SurnameTextBox.Text,
                        LastnameTextBox.Text,
                        EmailTextBox.Text,
                        PasswordBox.Password,
                        phoneNumber,
                        address,
                        bankCard,
                        (int)RoleComboBox.SelectedValue,
                        _userId.Value,
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
                }

                _parentWindow.LoadData();
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка: {ex.Message}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ValidateInput()
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

            if (PasswordBox.Password != ConfirmPasswordBox.Password)
                throw new Exception("Пароли не совпадают");

            if (RoleComboBox.SelectedValue == null)
                throw new Exception("Выберите роль пользователя");
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}