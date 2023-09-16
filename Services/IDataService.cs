using WebApi.Models;

namespace WebApi.Services
{
    public interface IDataService
    {
        List<Data> Get();
        Data Get(string id);
        Data Create(Data data);
        void Update(string id, Data data);
        void Remove(string id);
    }
}
