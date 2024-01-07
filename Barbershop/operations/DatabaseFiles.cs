using System;
using System.IO;
using System.Xml.Linq;

namespace Barbershop
{
    public sealed class DatabaseFiles
    {
        public enum Document
        {
            Clients, Hairs, Works
        }

        private static string GetFilename(Document document)
        {
            switch (document)
            {
                case Document.Clients: return "Clients.xml";
                case Document.Hairs: return "Hairs.xml";
                case Document.Works: return "Works.xml";
                default: throw new ArgumentException("Передали некорректный аргумент");
            }
        }

        internal static string GetPath(Document document) => Path.Combine("../../", GetFilename(document));

        internal static XDocument Load(Document document) => XDocument.Load(GetPath(document));
    }
}
