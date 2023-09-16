namespace WebApi.Models
{
    public interface IOCRDataCollectDatabaseSettings
    {
        string DataCollectionName {get; set; }
        string UsersCollectionName { get; set; }
        string DatabaseName {get; set; }
        string ConnectionString { get; set; }
    }
}
