namespace Barbershop.operations
{
    internal interface IOperatorLoader
    {
        IOperator LoadOperator(DatabaseFiles.Document currentDocument);
    }
}
