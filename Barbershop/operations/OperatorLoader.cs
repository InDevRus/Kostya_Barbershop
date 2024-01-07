using System;

namespace Barbershop.operations
{
    public class OperatorLoader : IOperatorLoader
    {
        private readonly HairsOperator hairsOperator = new HairsOperator();
        private readonly ClientsOperator clientOperator = new ClientsOperator();
        private readonly WorksOperator worksOperator = new WorksOperator();

        public IOperator LoadOperator(DatabaseFiles.Document currentDocument)
        {
            switch (currentDocument)
            {
                case DatabaseFiles.Document.Hairs: return hairsOperator;
                case DatabaseFiles.Document.Clients: return clientOperator;
                case DatabaseFiles.Document.Works: return worksOperator;
                default: throw new InvalidOperationException();
            }
        }
    }
}
