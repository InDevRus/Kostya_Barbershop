using Barbershop.operations;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Xml.Linq;

namespace Barbershop
{
    using static Barbershop.DatabaseFiles;

    /// <summary>
    /// Логика взаимодействия для Glavform.xaml
    /// </summary>
    public partial class Glavform : Window
    {
        private readonly IOperatorLoader operatorLoader = new OperatorLoader();

        private Document CurrentWorkingDocument { get; set; }

        private IOperator CurrentOperator => operatorLoader.LoadOperator(CurrentWorkingDocument);

        public Glavform()
        {
            InitializeComponent();
        }

        private void dg_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void MenuItem1_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkingDocument = Document.Hairs;

            dobavlenie1.Visibility = Visibility.Visible;
            dobavlenie2.Visibility = Visibility.Visible;
            dobavlenie3.Visibility = Visibility.Visible;
            dobavlenie4.Visibility = Visibility.Visible;
            dobavlenie5.Visibility = Visibility.Hidden;
            dobavlenie6.Visibility = Visibility.Hidden;
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            dg.Visibility = Visibility.Visible;
            Hairs.Visibility = Visibility.Visible;
            Clients.Visibility = Visibility.Hidden;
            Works.Visibility = Visibility.Hidden;

            CurrentOperator.Reload(this);
        }

        private void MenuItem2_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkingDocument = Document.Clients;

            dobavlenie1.Visibility = Visibility.Visible;
            dobavlenie2.Visibility = Visibility.Visible;
            dobavlenie3.Visibility = Visibility.Visible;
            dobavlenie4.Visibility = Visibility.Visible;
            dobavlenie5.Visibility = Visibility.Visible;
            dobavlenie6.Visibility = Visibility.Visible;
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            dg.Visibility = Visibility.Visible;
            Hairs.Visibility = Visibility.Hidden;
            Clients.Visibility = Visibility.Visible;
            Works.Visibility = Visibility.Hidden;

            CurrentOperator.Reload(this);
        }

        private void MenuItem3_Click(object sender, RoutedEventArgs e)
        {
            CurrentWorkingDocument = Document.Works;

            dobavlenie1.Visibility = Visibility.Visible;
            dobavlenie2.Visibility = Visibility.Visible;
            dobavlenie3.Visibility = Visibility.Visible;
            dobavlenie4.Visibility = Visibility.Visible;
            dobavlenie5.Visibility = Visibility.Hidden;
            dobavlenie6.Visibility = Visibility.Hidden;
            dg.Visibility = Visibility.Visible;
            img1.Visibility = Visibility.Hidden;
            img2.Visibility = Visibility.Hidden;
            Hairs.Visibility = Visibility.Hidden;
            Clients.Visibility = Visibility.Hidden;
            Works.Visibility = Visibility.Visible;

            CurrentOperator.Reload(this);
        }

        private void MenuItemAdd_Click(object sender, RoutedEventArgs e)
        {
            add.Visibility = Visibility.Visible;
            edit.Visibility = Visibility.Hidden;
            del.Visibility = Visibility.Hidden;
            search.Visibility = Visibility.Hidden;
        }

        private void MenuItemEdit_Click(object sender, RoutedEventArgs e)
        {
            add.Visibility = Visibility.Hidden;
            edit.Visibility = Visibility.Visible;
            del.Visibility = Visibility.Hidden;
            search.Visibility = Visibility.Hidden;
        }

        private void MenuItemDel_Click(object sender, RoutedEventArgs e)
        {
            add.Visibility = Visibility.Hidden;
            edit.Visibility = Visibility.Hidden;
            del.Visibility = Visibility.Visible;
            search.Visibility = Visibility.Hidden;
        }

        private void MenuItemSearch_Click(object sender, RoutedEventArgs e)
        {
            add.Visibility = Visibility.Hidden;
            edit.Visibility = Visibility.Hidden;
            del.Visibility = Visibility.Hidden;
            search.Visibility = Visibility.Visible;
        }

        private bool validateAdditionCode()
        {
            if (dobavlenie1.Text.Length == 0)
            {
                MessageBox.Show("Требуется непустой код.");
                return false;
            }

            if (!int.TryParse(dobavlenie1.Text, out var _))
            {
                MessageBox.Show("Код должен быть числом.");
                return false;
            }

            return true;
        }

        private void ClearFields()
        {
            dobavlenie1.Clear();
            dobavlenie2.Clear();
            dobavlenie3.Clear();
            dobavlenie4.Clear();
            dobavlenie5.Clear();
            dobavlenie6.Clear();
        }

        private void buttonadd_Click(object sender, RoutedEventArgs e)
        {
            if (!validateAdditionCode()) 
            {
                return;
            }

            CurrentOperator.Add(this);
        }

        private void buttondel_Click(object sender, RoutedEventArgs e)
        {
            if (!validateAdditionCode())
            {
                return;
            }

            CurrentOperator.Remove(this);

            ClearFields();
        }

        private void buttonedit_Click(object sender, RoutedEventArgs e)
        {
            if (!validateAdditionCode())
            {
                return;
            }

            CurrentOperator.Edit(this);

            ClearFields();
        }

        private void buttonsearch_Click(object sender, RoutedEventArgs e)
        {
            CurrentOperator.Search(this);
        }
    }
}

