using System.Windows;

namespace Barbershop
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string username = "1";
        private string pass = "123";
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (loginbox.Text == username & passwordbox.Password == pass)
            {
                Glavform gf = new Glavform();
                gf.ShowDialog();
            }
            else
            {
                MessageBox.Show("Неправильный логин или пароль");
            }
        }
    }
}
