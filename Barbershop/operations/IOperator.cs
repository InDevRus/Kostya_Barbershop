using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Barbershop.operations
{
    public abstract class IOperator
    {
        public abstract void Reload(Glavform window);

        public abstract void Add(Glavform window);

        public abstract void Remove(Glavform window);

        public abstract void Edit(Glavform window);

        public abstract void Search(Glavform window);

        protected abstract IEnumerable<XElement> SearchByKey(Glavform window);

        protected bool CheckPresenceOfMainKey(Glavform window) => SearchByKey(window).Any();

        protected abstract IEnumerable<(System.Windows.Controls.TextBox, string)> ProvideSearchCriteriaPairs(Glavform window, XElement entry);

        protected virtual bool CheckEntryMeetsCriteria(IEnumerable<(System.Windows.Controls.TextBox, string)> criteriaPairs)
        {
            return criteriaPairs.All(pair =>
            {
                var textBoxValue = pair.Item1?.Text;
                var entryText = pair.Item2;
                return textBoxValue == null || textBoxValue == "" || textBoxValue == entryText;
            });
        }
    }
}
