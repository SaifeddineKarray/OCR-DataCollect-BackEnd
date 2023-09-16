using WebApi.Models;
using MongoDB.Driver;

namespace WebApi.Services
{
    public class DataService : IDataService
    {
        private IMongoCollection<Data> _data;

        public DataService(IOCRDataCollectDatabaseSettings settings, IMongoClient mongoclient)
        {
            var database = mongoclient.GetDatabase(settings.DatabaseName);
            _data = database.GetCollection<Data>(settings.DataCollectionName);
        }
        public Data Create(Data data)
        {
            _data.InsertOne(data);
            return data;
        }

        public List<Data> Get()
        {
            return _data.Find(data=>true).ToList();
        }

        public Data Get(string id)
        {
            return _data.Find(data=>data.Id == id).FirstOrDefault();
        }

        public void Remove(string id)
        {
            _data.DeleteOne(data => data.Id == id);
        }

        public void Update(string id, Data data)
        {
            _data.ReplaceOne(data => data.Id == id, data);
        }
    }
}
