using System.Windows;

namespace UserManagementApp
{
    public partial class MainMenu : Window
    {
        public MainMenu()
        {
            InitializeComponent();
            this.Closing += (s, e) => Application.Current.Shutdown();
        }

        private void UsersButton_Click(object sender, RoutedEventArgs e)
        {
            UsersWindow usersWindow = new UsersWindow();
            usersWindow.Show();
            this.Hide();
        }

        private void RolesButton_Click(object sender, RoutedEventArgs e)
        {
            RolesWindow rolesWindow = new RolesWindow();
            rolesWindow.Show();
            this.Hide();
        }
    }
}