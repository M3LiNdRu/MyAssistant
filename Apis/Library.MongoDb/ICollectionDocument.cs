namespace Library.MongoDb
{
    public interface ICollectionDocument<T>
    {
        T Id { get; set; }
    }
}
