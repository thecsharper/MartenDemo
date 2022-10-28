using MartenDemo.Models;

namespace MartenDemo
{
    public interface IMartenQueries
    {
        MartenData GetSingleItem(Guid id);

        List<MartenData> GetManyItems(Guid id);
    }
}