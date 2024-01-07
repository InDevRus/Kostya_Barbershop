using System;
using System.Collections.ObjectModel;
using static Barbershop.DatabaseFiles;
using System.Xml.Linq;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Input;

namespace Barbershop.operations
{
    internal class HairsOperator : IOperator
    {
        private XDocument HairsDocument => Load(Document.Hairs);

        private readonly string filename = GetPath(Document.Hairs);

        public override void Reload(Glavform window)
        {
            var document = HairsDocument;
            var hairs = document
                           .Element("Hairs")
                           .Elements("Hair")
                           .OrderBy(hairElement => int.Parse(hairElement.Element("KodS").Value))
                           .Select(hairElement => new
                           {
                               Код = hairElement.Element("KodS").Value,
                               Название = hairElement.Element("Name").Value,
                               Пол = hairElement.Element("Gender").Value,
                               Цена = hairElement.Element("Price").Value
                           })
                           .ToList();

            var HairsCollection = new ObservableCollection<object>(hairs);
            window.dg.ItemsSource = HairsCollection;
        }

        public override void Add(Glavform window)
        {
            var document = HairsDocument;

            document.Element("Hairs").Add(
                new XElement("Hair",
                    new XElement("KodS", window.dobavlenie1.Text),
                    new XElement("Name", window.dobavlenie2.Text),
                    new XElement("Gender", window.dobavlenie3.Text),
                    new XElement("Price", window.dobavlenie4.Text))
                );

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

            var document = HairsDocument;

            var firstHair = document
                .Elements("Hairs")
                .Elements("Hair")
                .First(b => ((string)b.Element("KodS")) == window.dobavlenie1.Text);

            firstHair.SetElementValue("Name", window.dobavlenie2.Text);
            firstHair.SetElementValue("Gender", window.dobavlenie3.Text);
            firstHair.SetElementValue("Price", window.dobavlenie4.Text);

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

            var document = HairsDocument;
            
            document
                .Elements("Hairs")
                .Elements("Hair")
                .First(b => ((string)b.Element("KodS")) == window.dobavlenie1.Text)
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
                var kod = (from x in HairsDocument.Element("Hairs").Elements("Hair")
                           where (string)x.Element("KodS") == dobavlenie1.Text || (string)x.Element("Name") == dobavlenie2.Text || (string)x.Element("Gender") == dobavlenie3.Text || (string)x.Element("Price") == dobavlenie4.Text
                           select new
                           {
                               Код = x.Element("KodS").Value,
                               Название = x.Element("Name").Value,
                               Пол = x.Element("Gender").Value,
                               Цена = x.Element("Price").Value

                           }).ToList();

                dg.ItemsSource = kod;
            }
            if (dobavlenie1.Text == "" && dobavlenie2.Text == "группировка" && dobavlenie3.Text == "" && dobavlenie4.Text == "")
            {
                var type = (from x in HairsDocument.Element("Hairs").Elements("Hair")
                            group x by x.Element("Name").Value into g
                            select new
                            {
                                Прически = g.Key
                            }).ToList();

                dg.ItemsSource = type;
            }

            if (dobavlenie1.Text == "" && dobavlenie3.Text != null && dobavlenie2.Text == "" && dobavlenie4.Text == "")
            {
                var gen = (from x in HairsDocument.Element("Hairs").Elements("Hair")
                           where (string)x.Element("Gender") == dobavlenie3.Text
                           group x by x.Element("Gender").Value into g
                           select new
                           {
                               Прически = g.Key,
                               Количество = g.Count()
                           }).ToList();

                dg.ItemsSource = gen;
            }

            */

            var document = HairsDocument;
            var result = document
                .Element("Hairs")
                .Elements("Hair")
                .Where(hairEntry => CheckEntryMeetsCriteria(ProvideSearchCriteriaPairs(window, hairEntry)))
                .Select(hairEntry => new {
                    Код = hairEntry.Element("KodS").Value,
                    Название = hairEntry.Element("Name").Value,
                    Пол = hairEntry.Element("Gender").Value,
                    Цена = hairEntry.Element("Price").Value
                })
                .ToList();

            window.dg.ItemsSource = result;
        }

        protected override IEnumerable<XElement> SearchByKey(Glavform window) => HairsDocument
                .Elements("Hairs")
                .Elements("Hair")
                .Where(hairElement => (string)hairElement.Element("KodS") == window.dobavlenie1.Text);

        protected override IEnumerable<(TextBox, string)> ProvideSearchCriteriaPairs(Glavform window, XElement hairEntry)
        {
            return new List<(TextBox, string)> {
                (window.dobavlenie1, (string)hairEntry.Element("KodS")),
                (window.dobavlenie2, (string)hairEntry.Element("Name")),
                (window.dobavlenie3, (string)hairEntry.Element("Gender")),
                (window.dobavlenie4, (string)hairEntry.Element("Price")),
            };
        }
    }
}
