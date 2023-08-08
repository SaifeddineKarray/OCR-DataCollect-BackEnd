namespace WebAPI.Models
{
    public interface IOCRDataCollectDatabaseSettings
    {
        string DataCollectionName {get; set; }
        string DatabaseName {get; set; }
        string ConnectionString { get; set; }
    }
}
