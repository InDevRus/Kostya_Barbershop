using static Barbershop.DatabaseFiles;
using System.Xml.Linq;
using System.Windows;
using System.Collections.ObjectModel;
using System.Linq;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Barbershop.operations
{
    internal class ClientsOperator : IOperator
    {
        private XDocument ClientsDocument => Load(Document.Clients);

        private readonly string filename = GetPath(Document.Clients);

        public override void Reload(Glavform window)
        {
            var document = ClientsDocument;
            var clients = document
                .Element("Clients")
                .Elements("Client")
                .OrderBy(clientElement => int.Parse(clientElement.Element("KodC").Value))
                .Select(clientElement => new
                {
                    Код = clientElement.Element("KodC").Value,
                    Фамилия = clientElement.Element("Surname").Value,
                    Имя = clientElement.Element("NameC").Value,
                    Отчество = clientElement.Element("MiddleName").Value,
                    Пол = clientElement.Element("Gender").Value,
                    ПостКлиент = clientElement.Element("PostC").Value

                })
                .ToList();


            var ClientsCollection = new ObservableCollection<object>(clients);
            window.dg.ItemsSource = ClientsCollection;
        }

        public override void Add(Glavform window)
        {
            var document = ClientsDocument;
            document.Element("Clients").Add(new XElement("Client",
                new XElement("KodC", window.dobavlenie1.Text),
                new XElement("Surname", window.dobavlenie2.Text),
                new XElement("NameC", window.dobavlenie3.Text),
                new XElement("MiddleName", window.dobavlenie4.Text),
                new XElement("Gender", window.dobavlenie5.Text),
                new XElement("PostC", window.dobavlenie6.Text)));

            document.Save(filename);

            MessageBox.Show("Новые данные добавлены!");

            Reload(window);
        }

        public override void Edit(Glavform window)
        {
            if (!CheckPresenceOfMainKey(window))
            {
                MessageBox.Show("Ошибка");
                return;
            }

            var document = ClientsDocument;

            var firstClient = document
                .Elements("Clients")
                .Elements("Client")
                .First(b => ((string)b.Element("KodC")) == window.dobavlenie1.Text);

            firstClient.SetElementValue("Surname", window.dobavlenie2.Text);
            firstClient.SetElementValue("NameC", window.dobavlenie3.Text);
            firstClient.SetElementValue("MiddleName", window.dobavlenie4.Text);
            firstClient.SetElementValue("Gender", window.dobavlenie5.Text);
            firstClient.SetElementValue("PostC", window.dobavlenie6.Text);

            document.Save(filename);

            Reload(window);

            MessageBox.Show("Данные отредактированы");
        }

        public override void Remove(Glavform window)
        {
            if (!CheckPresenceOfMainKey(window))
            {
                MessageBox.Show("Ошибка");
                return;
            }

            var document = ClientsDocument;

            document
                .Elements("Clients")
                .Elements("Client")
                .First(b => ((string)b.Element("KodC")) == window.dobavlenie1.Text)
                .Remove();

            document.Save(filename);

            Reload(window);

            MessageBox.Show("Данные удалены");
        }

        public override void Search(Glavform window)
        {
            /*
            if (dobavlenie1 != null)
            {
                var kod1 = (from x in ClientsDocument.Element("Clients").Elements("Client")
                            where (string)x.Element("KodC") == dobavlenie1.Text || (string)x.Element("Surname") == dobavlenie2.Text || (string)x.Element("NameC") == dobavlenie3.Text || (string)x.Element("MiddleName") == dobavlenie4.Text || (string)x.Element("Gender") == dobavlenie5.Text || (string)x.Element("PostC") == dobavlenie6.Text
                            select new
                            {
                                Код = x.Element("KodC").Value,
                                Фамилия = x.Element("Surname").Value,
                                Имя = x.Element("NameC").Value,
                                Отчество = x.Element("MiddleName").Value,
                                Пол = x.Element("Gender").Value,
                                ПостКлиент = x.Element("PostC").Value

                            }).ToList();

                dg.ItemsSource = kod1;
            }

            if (dobavlenie1.Text == "" && dobavlenie2.Text == "" && dobavlenie3.Text == "" && dobavlenie4.Text == "" && dobavlenie6.Text == "" && dobavlenie5.Text != null)
            {
                var gen = (from x in ClientsDocument.Element("Clients").Elements("Client")
                           where (string)x.Element("Gender") == dobavlenie5.Text
                           group x by x.Element("Gender").Value into g
                           select new
                           {
                               Гендер = g.Key,
                               Количество = g.Count()
                           }).ToList();

                dg.ItemsSource = gen;
            }

            if (dobavlenie1.Text == "" && dobavlenie2.Text == "первый" && dobavlenie3.Text == "" && dobavlenie4.Text == "" && dobavlenie6.Text == "" && dobavlenie2.Text != null && dobavlenie5.Text == "")
            {
                var gen = (from x in ClientsDocument.Element("Clients").Elements("Client")
                           where (string)x.Element("Surname") == dobavlenie2.Text
                           group x by x.Element("Surname").Value into g
                           select new
                           {
                               Фамилия = g.First().Element("Surname").Value,
                           }).ToList();

                dg.ItemsSource = gen;
            }
            */

            var document = ClientsDocument;

            var result = document
                .Element("Clients")
                .Elements("Client")
                .Where(clientEntry => CheckEntryMeetsCriteria(ProvideSearchCriteriaPairs(window, clientEntry)))
                .Select(clientEntry => new {
                    Код = clientEntry.Element("KodC").Value,
                    Фамилия = clientEntry.Element("Surname").Value,
                    Имя = clientEntry.Element("NameC").Value,
                    Отчество = clientEntry.Element("MiddleName").Value,
                    Пол = clientEntry.Element("Gender").Value,
                    ПостКлиент = clientEntry.Element("PostC").Value
                })
                .ToList();

            window.dg.ItemsSource = result;
        }

        protected override IEnumerable<XElement> SearchByKey(Glavform window) => ClientsDocument
                .Elements("Clients")
                .Elements("Client")
                .Where(clientElement => (string)clientElement.Element("KodC") == window.dobavlenie1.Text);

        protected override IEnumerable<(TextBox, string)> ProvideSearchCriteriaPairs(Glavform window, XElement clientEntry)
        {
            return new List<(TextBox, string)> {
                (window.dobavlenie1, (string)clientEntry.Element("KodC")),
                (window.dobavlenie2, (string)clientEntry.Element("Surname")),
                (window.dobavlenie3, (string)clientEntry.Element("NameC")),
                (window.dobavlenie4, (string)clientEntry.Element("MiddleName")),
                (window.dobavlenie5, (string)clientEntry.Element("Gender")),
                (window.dobavlenie6, (string)clientEntry.Element("PostC")),
            };
        }
    }
}
