using System.Collections.ObjectModel;
using static Barbershop.DatabaseFiles;
using System.Xml.Linq;
using System.Linq;
using System.Windows;
using System.Collections.Generic;
using System.Windows.Controls;

namespace Barbershop.operations
{
    internal class WorksOperator : IOperator
    {
        private XDocument WorksDocument => Load(Document.Works);

        private readonly string filename = GetPath(Document.Works);

        public override void Reload(Glavform window)
        {
            var document = WorksDocument;

            var works = document
                .Element("Works")
                .Elements("Work")
                .OrderBy(workElement => int.Parse(workElement.Element("KodR").Value))
                .Select(workElement => new
                {
                    Код_Работы = workElement.Element("KodR").Value,
                    Код_Стрижки = workElement.Element("KodS").Value,
                    Код_Клиента = workElement.Element("KodC").Value,
                    Дата = workElement.Element("Date").Value
                })
                .ToList();

            window.dg.ItemsSource = new ObservableCollection<object>(works);
        }

        public override void Add(Glavform window)
        {
            var document = WorksDocument;

            document
                .Element("Works")
                .Add(new XElement("Work",
                    new XElement("KodR", window.dobavlenie1.Text),
                    new XElement("KodS", window.dobavlenie2.Text),
                    new XElement("KodC", window.dobavlenie3.Text),
                    new XElement("Date", window.dobavlenie4.Text))
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

            var document = WorksDocument;

            var firstWork = document
                .Elements("Works")
                .Elements("Work")
                .First(b => ((string)b.Element("KodR")) == window.dobavlenie1.Text);

            firstWork.SetElementValue("KodS", window.dobavlenie2.Text);
            firstWork.SetElementValue("KodC", window.dobavlenie3.Text);
            firstWork.SetElementValue("Date", window.dobavlenie4.Text);

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

            var document = WorksDocument;

            document
                .Elements("Works")
                .Elements("Work")
                .First(worksElement => ((string)worksElement.Element("KodR")) == window.dobavlenie1.Text)
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
                var kods = (from x in WorksDocument.Element("Works").Elements("Work")
                            where (string)x.Element("KodR") == dobavlenie1.Text || (string)x.Element("KodS") == dobavlenie2.Text || (string)x.Element("KodC") == dobavlenie3.Text || (string)x.Element("Date") == dobavlenie4.Text
                            select new
                            {
                                Код_Работы = x.Element("KodR").Value,
                                Код_Стрижки = x.Element("KodS").Value,
                                Код_Клиента = x.Element("KodC").Value,
                                Дата = x.Element("Date").Value,

                            }).ToList();

                dg.ItemsSource = kods;
            }
            */

            var document = WorksDocument;
            
            var result = document
                .Element("Works")
                .Elements("Work")
                .Where(workEntry => CheckEntryMeetsCriteria(ProvideSearchCriteriaPairs(window, workEntry)))
                .Select(workEntry => new {
                    Код_Работы = workEntry.Element("KodR").Value,
                    Код_Стрижки = workEntry.Element("KodS").Value,
                    Код_Клиента = workEntry.Element("KodC").Value,
                    Дата = workEntry.Element("Date").Value,
                })
                .ToList();

            window.dg.ItemsSource = result;
        }

        protected override IEnumerable<XElement> SearchByKey(Glavform window) => WorksDocument
                .Elements("Works")
                .Elements("Work")
                .Where(workElement => (string)workElement.Element("KodR") == window.dobavlenie1.Text);

        protected override IEnumerable<(TextBox, string)> ProvideSearchCriteriaPairs(Glavform window, XElement workEntry)
        {
            return new List<(TextBox, string)> {
                (window.dobavlenie1, (string)workEntry.Element("KodR")),
                (window.dobavlenie2, (string)workEntry.Element("KodS")),
                (window.dobavlenie3, (string)workEntry.Element("KodC")),
                (window.dobavlenie4, (string)workEntry.Element("Date")),
            };
        }
    }
}
